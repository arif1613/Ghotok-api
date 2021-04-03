using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Ghotok.Data.Utils.Cache
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _cache;

        public CacheHelper(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public Task Add<T>(T o, string key, int minutes) where T : class
        {
            return Task.Run(() =>
            {
                MemoryCacheEntryOptions cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(minutes));
                if (!_cache.TryGetValue(key, out T cacheEntry))
                {
                    // Save data in cache.
                    _cache.Set(key, o, cacheEntryOptions);
                }
            });
        }


        public Task Clear(string key)
        {
            return Task.Run(() =>
            {
                if (_cache.TryGetValue(key, out dynamic cacheEntry))
                {
                    _cache.Remove(key);
                }
            });
        }


        public Task<bool> Exists(string key)
        {

            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    return false;
                }
                if (_cache.TryGetValue(key, out dynamic cacheEntry))
                {
                    return true;
                }

                return false;
            });

        }


        public async Task<T> Get<T>(string key) where T : class
        {

            return await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (_cache.TryGetValue(key, out T cacheEntry))
            {
                return cacheEntry;
            }
            return null;
        });

        }

        public Task Update<T>(T NewObject, string key) where T : class
        {
            return Task.Run(() =>
            {
                if (_cache.TryGetValue(key, out T cacheEntry))
                {
                    _cache.Set(key, NewObject);
                }
            });
        }

    }
}
