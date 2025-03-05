using StackExchange.Redis;
using T.Domain.Models;

namespace T.Domain.Interfaces {
    public interface IRedisService {

        IDatabase GetDatabase();

        Task<bool> KeyExistsAsync(string key);

        Task<RedisValue<T>> GetAsync<T>(string key);

        Task SetAsync(string key, object? data, TimeSpan? ttl = null);

        Task RemoveAsync(string key);

        Task SetValueAsync(string key, List<string> values, TimeSpan? ttl = null);

        Task RemoveSetValueAsync(string key, List<string> values, TimeSpan? ttl = null);

        List<string> GetSetValue(string key);

        Task<List<string>> GetSetValueAsync(string key);
    }

}
