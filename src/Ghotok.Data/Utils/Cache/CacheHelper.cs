using System;
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

        public void Add<T>(T o, string key, int minutes) where T : class
        {

            MemoryCacheEntryOptions cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(minutes));
            if (!_cache.TryGetValue(key, out T cacheEntry))
            {
                // Save data in cache.
                _cache.Set(key, o, cacheEntryOptions);
            }
        }


        public void Clear(string key)
        {

            if (_cache.TryGetValue(key, out dynamic cacheEntry))
            {
                _cache.Remove(key);
            }
        }


        public bool Exists(string key)
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

        }


        public T Get<T>(string key) where T : class
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

        }

        public void Update<T>(T NewObject, string key) where T : class
        {

            if (_cache.TryGetValue(key, out T cacheEntry))
            {
                _cache.Set(key, NewObject);
            }
        }

    }
}
