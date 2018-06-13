using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public interface ICache
    {
        bool AddItem(string key, object item, int seconds);

        T GetItem<T>(string key);

        bool DelItem(string key);

        Dictionary<string, T> GetAllItems<T>();

        bool UpdateItem(string key, object item, int seconds);
    }

    public class CacheFactory
    {
        public static ICache Create(CacheType type = CacheType.CustomCache, string region = null)
        {
            ICache cache = null;
            switch (type)
            {
                case CacheType.DefaultMemoryCache:
                    cache = new DefaultCache(region);
                    break;
                //case CacheType.Memcached:
                //    cache = new Memcache();
                //    break;
                case CacheType.Redis:
                    cache = new RedisCache();
                    break;
                default:
                    cache = new DefaultCache(region);
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
        Redis,
    }
}
