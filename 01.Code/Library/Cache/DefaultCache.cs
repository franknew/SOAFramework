using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using SOAFramework.Library.Cache;
using System.Collections.Specialized;

namespace SOAFramework.Library.Cache
{
    public class DefaultCache : ICache
    {
        private string region = "Memory";

        public DefaultCache(string regionName)
        {
            if (!string.IsNullOrEmpty(regionName))
            {
                region = regionName;
            }
        }

        public DefaultCache()
        {
        }

        private ObjectCache _cache = MemoryCache.Default;



        public DefaultCache(int seconds)
        {
        }

        public bool AddItem(string key, object item, int seconds = -1)
        {
            CacheItem cacheItem = new CacheItem(key, item, region);
            CacheItemPolicy policy = new CacheItemPolicy();
            if (seconds > 0)
            {
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(seconds);
            }
            _cache.Set(cacheItem, policy);
            return true;
        }

        public T GetItem<T>(string key)
        {
            return (T)_cache.GetCacheItem(key)?.Value;
        }

        public bool DelItem(string key)
        {
            _cache.Remove(key);
            return true;
        }

        public Dictionary<string, T> GetAllItems<T>()
        {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            foreach (var item in _cache)
            {
                CacheItem i = new CacheItem(item.Key, item.Value);
                dic[item.Key] = (T)item.Value; 
            }

            return dic;
        }

        public bool UpdateItem(string key, object value, int seconds = -1)
        {
            CacheItem cacheItem = new CacheItem(key, value);
            CacheItemPolicy policy = new CacheItemPolicy();
            if (seconds > 0)
            {
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(seconds);
            }
            if (_cache.Contains(key)) _cache.Set(cacheItem, policy);
            else AddItem(key, value);
            return true;
        }
    }
}
