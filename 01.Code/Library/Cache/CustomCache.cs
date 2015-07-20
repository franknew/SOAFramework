using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public class CustomCache : ICache
    {
        public CustomCache(string regionName = "default")
        {
            this.regionName = regionName;
        }

        public static CustomCache GetCache(string regionName)
        {
            CustomCache cache = null;
            lock (locker)
            {
                if (cacheMapDic.ContainsKey(regionName))
                {
                    cache = cacheMapDic[regionName];
                }
                else
                {
                    cache = new CustomCache(regionName);
                    cacheMapDic[regionName] = cache;
                }
            }
            return cache;
        }

        private string regionName = "default";

        private static readonly Dictionary<string, CustomCache> cacheMapDic = new Dictionary<string, CustomCache>();

        private static object locker = new object();

        private Dictionary<string, CustomCacheEntity> cacheItemDic = new Dictionary<string, CustomCacheEntity>();

        public static CustomCache Default
        {
            get
            {
                return GetCache("default");
            }
        }

        public bool AddItem(CacheItem item, int seconds)
        {
            throw new NotImplementedException();
        }

        public bool DelItem(string key)
        {
            throw new NotImplementedException();
        }

        public bool DelItem(CacheItem item)
        {
            throw new NotImplementedException();
        }

        public List<CacheItem> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public CacheItem GetItem(string key)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(CacheItem item)
        {
            throw new NotImplementedException();
        }
    }
}
