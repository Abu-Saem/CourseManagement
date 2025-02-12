using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Application
{
    public interface IRedisCacheService
    {
        void SetCache<T>(string key, T value, TimeSpan expiration);
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);

        T GetCache<T>(string key);
        Task<T> GetCacheAsync<T>(string key);

        void RemoveCache(string key);
        Task RemoveCacheAsync(string key);

    }
}
