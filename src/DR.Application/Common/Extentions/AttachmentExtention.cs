using System.Reflection;
using DR.Domain.Common.Interfaces;
using DR.Domain.Common.Models;
using DR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DR.Application.Common.Extentions {

    public class NotLoadFileAttribute : Attribute { }

    internal class IgnoredPropertyContractResolver : DefaultContractResolver {

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);
            property.Ignored = false;
            return property;
        }
    }

    public static class ObjectExtension {

        private static readonly JsonSerializerSettings serializer = new() {
            ContractResolver = new IgnoredPropertyContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static T? ToObjectIncludingIgnored<T>(this JObject jObject) {
            var data = jObject.ToObject<T>();

            foreach (var prop in typeof(T).GetProperties()) {
                if (Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute))) {
                    prop.SetValue(data, jObject[prop.Name]?.ToObject(prop.PropertyType));
                }
            }

            return data;
        }

        public static JObject UpdateProperty(this JObject json, string name, object value) {
            JToken token = JToken.FromObject(value, JsonSerializer.Create(serializer));
            if (json.ContainsKey(name)) json.Remove(name);
            json.TryAdd(name, token);
            return json;
        }

        public static JObject ToJson<T>(this T data)
            where T : class {
            var json = JObject.FromObject(data, JsonSerializer.Create(serializer));
            return json;
        }
    }

    public static class AttachmentExtention {

        public static async Task<T> FileAsync<T>(this T model, DrContext db, CancellationToken token) where T : class {
            var files = (await model.GetAttachmentFiles(db, token)).GroupBy(o => o.ParentId.ToString()).ToDictionary(k => k.Key, v => v.ToList());
            var json = model.ToJson();

            if (typeof(T).IsClass && typeof(IAttachment).IsAssignableFrom(typeof(T))) {
                var id = json.GetValue(nameof(IAttachment.Id))?.ToString();
                if (!string.IsNullOrEmpty(id)) {
                    json.UpdateProperty(nameof(IAttachment.Attachments), files.GetValueOrDefault(id, []));
                }
            }

            foreach (var prop in typeof(T).GetProperties()) {
                if (typeof(IAttachment).IsAssignableFrom(prop.PropertyType)) {
                    var data = (JObject?)json.GetValue(nameof(IAttachment.Id));
                    if (data != null) {
                        var id = data.GetValue(nameof(IAttachment.Id))?.ToString();
                        if (!string.IsNullOrEmpty(id)) {
                            data.UpdateProperty(nameof(IAttachment.Attachments), files.GetValueOrDefault(id, []));
                        }
                    }
                    json.UpdateProperty(prop.Name, data ?? []);
                } else if (IsListOfIMapAttachment(prop.PropertyType)) {
                    var data = json.GetValue(prop.Name) as JArray;
                    foreach (JObject item in (data ?? []).Cast<JObject>()) {
                        var id = item.GetValue(nameof(IAttachment.Id))?.ToString();
                        if (!string.IsNullOrEmpty(id)) {
                            item.UpdateProperty(nameof(IAttachment.Attachments), files.GetValueOrDefault(id, []));
                        }
                    }
                    json.UpdateProperty(prop.Name, data ?? []);
                }
            }

            return json.ToObject<T>()!;
        }

        public static async Task<List<T>> FileAsync<T>(this IQueryable<T> query, DrContext db, CancellationToken token)
            where T : IAttachment {
            var data = await query.ToListAsync(token);
            return await data.FileAsync(db, token);
        }

        public static async Task<List<T>> FileAsync<T>(this List<T> data, DrContext db, CancellationToken token)
            where T : IAttachment {
            var ids = data.Select(o => o.Id.ToString()).ToList();
            var attachments = await db.Attachments.AsNoTracking().Where(o => ids.Contains(o.ParentId.ToString()))
                .Select(o => AttachmentDto.FromEntity(o)).ToListAsync(token);
            var fileMaps = attachments.GroupBy(o => o.ParentId.ToString()).ToDictionary(o => o.Key, o => o.ToList());

            foreach (var item in data) {
                item.Attachments = fileMaps!.GetValueOrDefault(item.Id.ToString(), []);
            }

            return data;
        }

        // Private methods
        private static bool IsListOfIMapAttachment(Type type) {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
                Type elementType = type.GetGenericArguments()[0];
                return typeof(IAttachment).IsAssignableFrom(elementType);
            }
            return false;
        }

        private static async Task<List<AttachmentDto>> GetAttachmentFiles<T>(this T model, DrContext db, CancellationToken token)
            where T : class {
            var list = new List<string>();
            if (typeof(T).IsClass && typeof(IAttachment).IsAssignableFrom(typeof(T))) {
                var id = ((IAttachment)model).Id;
                if (id != null) list.Add(id.ToString()!);
            }

            foreach (var prop in model.GetType().GetProperties()) {
                if (typeof(IAttachment).IsAssignableFrom(prop.PropertyType)) {
                    var value = prop.GetValue(model) as IAttachment;
                    if (value?.Id != null) list.Add(value.Id.ToString()!);
                } else if (IsListOfIMapAttachment(prop.PropertyType)) {
                    var arr = prop.GetValue(model) as IEnumerable<object>;
                    foreach (IAttachment item in (arr ?? []).Cast<IAttachment>()) {
                        if (item.Id != null) {
                            list.Add(item.Id.ToString()!);
                        }
                    }
                }
            }

            return await db.Attachments.Where(o => list.Contains(o.ParentId.ToString()))
                .Select(o => AttachmentDto.FromEntity(o)).ToListAsync(token);
        }
    }
}
