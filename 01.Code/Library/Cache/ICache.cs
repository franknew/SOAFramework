using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public interface ICache
    {
        bool AddItem(CacheItem item, int seconds);

        CacheItem GetItem(string key);

        bool DelItem(CacheItem item);

        bool DelItem(string key);

        List<CacheItem> GetAllItems();

        bool UpdateItem(CacheItem item);
    }

    public class CacheFactory
    {
        public static ICache Create(CacheType type = CacheType.CustomCache, string region = "default")
        {
            ICache cache = null;
            switch (type)
            {
                case CacheType.DefaultMemoryCache:
                    cache = new DefaultCache(region);
                    break;
                case CacheType.Memcached:
                    cache = new Memcache();
                    break;
                default:
                    cache = CustomCache.GetCache(region);
                    break;
            }
            return cache;
        }
    }

    public enum CacheType
    {
        CustomCache,
        DefaultMemoryCache,
        Memcached,
    }
}
