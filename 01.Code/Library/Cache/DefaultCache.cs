using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using SOAFramework.Library.Cache;

namespace SOAFramework.Library.Cache
{
    public class DefaultCache : ICache
    {
        public const string Region = "Default";

        private MemoryCache _cache = MemoryCache.Default;
        CacheItemPolicy policy = new CacheItemPolicy();

        public DefaultCache()
        {
        }

        public DefaultCache(int seconds)
        {
            policy.SlidingExpiration = new TimeSpan(seconds * 1000);
        }

        public void AddItem(CacheItem item, int seconds = -1)
        {
            if (seconds > 0)
            {
                policy.SlidingExpiration = new TimeSpan(seconds * 1000);
            }
            _cache.Add(item, policy);
        }

        public CacheItem GetItem(string key)
        {
            return _cache.GetCacheItem(key, Region);
        }

        public void DelItem(CacheItem item)
        {
            _cache.Remove(item.Key, Region);
        }

        public void DelItem(string key)
        {
            _cache.Remove(key, Region);
        }

        public List<CacheItem> GetAllItems()
        {
            List<CacheItem> list = new List<CacheItem>();
            foreach (var item in _cache)
            {
                CacheItem i = new CacheItem(item.Key, item.Value);
            }

            return list;
        }

        public void UpdateItem(CacheItem item)
        {
            _cache.Set(item.Key, item.Value, policy, Region);
        }
    }
}
