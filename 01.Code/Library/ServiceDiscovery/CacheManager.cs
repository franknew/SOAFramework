using SOAFramework.Library.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library
{
    public class CacheManager<T>
    {
        private static string _cacheKey;
        public CacheManager(string cacheKey)
        {
            _cacheKey = cacheKey;
        }
        
        private ICache _cacheHandler = CacheFactory.Create(CacheType.DefaultMemoryCache);

        public T GetItem(string key)
        {
            T model = default(T);
            var item = _cacheHandler.GetItem<Dictionary<string, T>>(_cacheKey);
            if (item == null) return model;
            var dicCache = item;
            if (dicCache.ContainsKey(key)) model = dicCache[key];
            return model;
        }

        public void SetItem(string key, T t)
        {
            var item = _cacheHandler.GetItem<Dictionary<string, T>>(_cacheKey);
            Dictionary<string, T> dicCache;
            if (item == null)
            {
                dicCache = new Dictionary<string, T>();
                dicCache[key] = t;
                _cacheHandler.AddItem(_cacheKey, dicCache, -1);
            }
            else
            {
                dicCache = item;
                dicCache[key] = t;
                _cacheHandler.UpdateItem(_cacheKey, dicCache);
            }
        }

        public bool ContainsKey(string key)
        {
            var item = GetItem(key);
            return item != null;
        }

        public List<T> GetList()
        {
            var item = _cacheHandler.GetItem<Dictionary<string, T>>(_cacheKey);
            var dicCache = item;
            List<T> list = new List<T>();
            if (dicCache == null) return list;
            foreach (var key in dicCache.Keys)
            {
                list.Add(dicCache[key]);
            }

            return list;
        }
    }
}
