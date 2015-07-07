using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public interface ICache
    {
        void AddItem(CacheItem item, int seconds);

        CacheItem GetItem(string key);

        void DelItem(CacheItem item);

        void DelItem(string key);

        List<CacheItem> GetAllItems();

        void UpdateItem(CacheItem item);
    }

    public class CacheFactory
    {
        public static ICache Create(CacheType type)
        {
            ICache cache = null;
            switch (type)
            {
                case CacheType.DefaultMemoryCache:
                    cache = new DefaultCache();
                    break;
                case CacheType.Memcached:
                    cache = new Memcache();
                    break;
            }
            return cache;
        }
    }

    public enum CacheType
    {
        DefaultMemoryCache,
        Memcached,
    }
}
