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

        public DefaultCache(string regionName = null)
        {
            region = regionName;
        }

        public DefaultCache()
        {
        }

        private ObjectCache _cache = MemoryCache.Default;

        CacheItemPolicy policy = new CacheItemPolicy();


        public DefaultCache(int seconds)
        {
            policy.SlidingExpiration = new TimeSpan(seconds * 1000);
        }

        public bool AddItem(string key, object item, int seconds = -1)
        {
            if (seconds > 0) policy.SlidingExpiration = new TimeSpan(seconds * 1000);
            return _cache.Add(key, item, policy, region);
        }

        public T GetItem<T>(string key)
        {
            return (T)_cache.GetCacheItem(key, region)?.Value;
        }

        public bool DelItem(string key)
        {
            _cache.Remove(key, region);
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

        public bool UpdateItem(string key, object value)
        {
            if (_cache.Contains(key)) _cache.Set(key, value, policy, region);
            else AddItem(key, value);
            return true;
        }
    }
}
