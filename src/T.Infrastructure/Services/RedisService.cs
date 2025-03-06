using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using T.Domain.Interfaces;
using T.Domain.Models;

namespace T.Infrastructure.Services {
    public class RedisService(IServiceProvider serviceProvider) : IRedisService {
        private readonly IConnectionMultiplexer connection = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

        public IDatabase GetDatabase() {
            return this.connection.GetDatabase();
        }

        public async Task<bool> KeyExistsAsync(string key) {
            var redisCache = this.connection.GetDatabase();
            return await redisCache.KeyExistsAsync(key);
        }

        public async Task<RedisValue<T>> GetAsync<T>(string key) {
            var redisCache = this.connection.GetDatabase();

            var existed = await redisCache.KeyExistsAsync(key);
            if (!existed) return new RedisValue<T>();

            var result = await redisCache.StringGetWithExpiryAsync(key);
            return new RedisValue<T> {
                Value = result.Value.HasValue ? JsonConvert.DeserializeObject<T>(result.Value.ToString()) : default,
                Expiry = result.Expiry,
            };
        }

        public async Task RemoveAsync(string key) {
            var redisCache = this.connection.GetDatabase();
            await redisCache.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, object? data, TimeSpan? ttl = null) {
            var redisCache = this.connection.GetDatabase();
            var json = JsonConvert.SerializeObject(data);
            await redisCache.StringSetAsync(key, json);
            if (ttl.HasValue && ttl.Value > TimeSpan.Zero)
                await redisCache.KeyExpireAsync(key, ttl);
        }

        public async Task SetValueAsync(string key, List<string> values, TimeSpan? ttl = null) {
            var redisCache = this.connection.GetDatabase();
            var redisExisted = await redisCache.KeyExistsAsync(key);
            if (redisExisted) {
                await redisCache.KeyDeleteAsync(key);
            }

            if (values.Count == 0) return;

            var redisValues = values.Select(value => (RedisValue)value).ToArray();
            await redisCache.SetAddAsync(key, redisValues);
            if (ttl.HasValue && ttl.Value > TimeSpan.Zero)
                await redisCache.KeyExpireAsync(key, ttl);
        }

        public async Task RemoveSetValueAsync(string key, List<string> values, TimeSpan? ttl = null) {
            var redisCache = this.connection.GetDatabase();
            var redisExisted = await redisCache.KeyExistsAsync(key);
            if (!redisExisted) return;
            if (values.Count == 0) return;

            var redisValues = values.Select(value => (RedisValue)value).ToArray();
            await redisCache.SetRemoveAsync(key, redisValues);
            if (ttl.HasValue && ttl.Value > TimeSpan.Zero)
                await redisCache.KeyExpireAsync(key, ttl);
        }

        public List<string> GetSetValue(string key) {
            var redisCache = this.connection.GetDatabase();
            var members = redisCache.SetMembers(key);
            return members.Select(m => m.ToString()).ToList();
        }

        public async Task<List<string>> GetSetValueAsync(string key) {
            var redisCache = this.connection.GetDatabase();
            var members = await redisCache.SetMembersAsync(key);
            return members.Select(m => m.ToString()).ToList();
        }
    }

}
