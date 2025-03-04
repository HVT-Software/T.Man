using System.Diagnostics.CodeAnalysis;
using T.Domain.Entities;

namespace T.Api.Models {

    public class RoleDto {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionDto>? Permissions { get; set; }

        [return: NotNullIfNotNull(nameof(entity))]
        public static RoleDto? FromEntity(Role? entity, List<Permission>? permissions = null) {
            if (entity == null) return default;

            var result = new RoleDto {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
            };

            if (permissions != null) {
                result.Permissions = PermissionDto.FromEntities(permissions);
                result.Permissions = IncludePermissions(result.Permissions, entity);
            }

            return result;
        }

        private static List<PermissionDto> IncludePermissions(List<PermissionDto> permissions, Role role, bool isEnable = true) {
            if (role.RolePermissions == null || role.RolePermissions.Count == 0) return permissions;
            if (permissions.Count == 0) return permissions;

            foreach (var item in permissions) {
                var rolePermission = role.RolePermissions.FirstOrDefault(o => o.PermissionId == item.Id);
                if (rolePermission == null) continue;
                item.IsEnable = isEnable && rolePermission.IsEnable;
                item.Items = IncludePermissions(item.Items, role, item.IsEnable);
            }
            return permissions;
        }
    }
}
