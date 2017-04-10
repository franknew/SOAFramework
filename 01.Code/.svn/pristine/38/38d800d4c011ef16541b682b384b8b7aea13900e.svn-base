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

        public bool AddItem(CacheItem item, int seconds = -1)
        {
            if (seconds > 0)
            {
                policy.SlidingExpiration = new TimeSpan(seconds * 1000);
            }
            return _cache.Add(item.Key, item.Value, policy, region);
        }

        public CacheItem GetItem(string key)
        {
            return _cache.GetCacheItem(key, region);
        }

        public bool DelItem(CacheItem item)
        {
            _cache.Remove(item.Key, region);
            return true;
        }

        public bool DelItem(string key)
        {
            _cache.Remove(key, region);
            return true;
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

        public bool UpdateItem(CacheItem item)
        {
            _cache.Set(item.Key, item.Value, policy, region);
            return true;
        }
    }
}
