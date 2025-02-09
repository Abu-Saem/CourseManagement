using CourseManagement.Application;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CourseManagement.Infrastructure
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly StackExchange.Redis.IDatabase _cacheDb;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            await _cacheDb.StringSetAsync(key, serializedData, expiration);
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var data = await _cacheDb.StringGetAsync(key);
            return data.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(data);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
}
