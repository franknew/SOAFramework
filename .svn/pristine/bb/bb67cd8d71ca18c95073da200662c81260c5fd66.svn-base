using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Timers;

namespace SOAFramework.Library.Cache
{
    public class CustomCache : ICache
    {
        private Timer timer = new Timer(1000);

        public CustomCache(string regionName = "default")
        {
            this.regionName = regionName;
            //计时清除过期项
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var key in cacheMapDic.Keys)
            {
                var items = cacheMapDic[key];
                lock (locker)
                {
                    foreach (var itemkey in items.cacheItemDic.Keys)
                    {
                        if (DateTime.Now > items.cacheItemDic[itemkey].ExpiredTime)
                        {
                            items.cacheItemDic.Remove(itemkey);
                        }
                    }
                }
            }
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
            CustomCacheEntity entity = new CustomCacheEntity
            {
                ExpiredSeconds = seconds,
                ExpiredTime = DateTime.Now.AddSeconds(seconds),
                Item = item,
                Key = item.Key,
                Value = item.Value,
            };
            cacheItemDic[item.Key] = entity;
            return true;
        }

        public bool DelItem(string key)
        {
            if (!cacheItemDic.ContainsKey(key))
            {
                return false;
            }
            cacheItemDic.Remove(key);
            return true;
        }

        public bool DelItem(CacheItem item)
        {
            return DelItem(item.Key);
        }

        public List<CacheItem> GetAllItems()
        {
            List<CacheItem> list = new List<CacheItem>();
            foreach (var key in cacheItemDic.Keys)
            {
                list.Add(cacheItemDic[key].Item);
            }
            return list;
        }

        public CacheItem GetItem(string key)
        {
            CacheItem item = null;
            if (cacheItemDic.ContainsKey(key))
            {
                var entity = cacheItemDic[key];
                if (entity.ExpiredTime >= DateTime.Now)
                {
                    item = entity.Item;
                    entity.ExpiredTime = DateTime.Now.AddSeconds(entity.ExpiredSeconds);
                    cacheItemDic[key] = entity;
                }
                else
                {
                    cacheItemDic.Remove(key);
                }
            }
            return item;
        }

        public bool UpdateItem(CacheItem item)
        {
            if (!cacheItemDic.ContainsKey(item.Key))
            {
                return false;
            }
            var entity = cacheItemDic[item.Key];
            entity.Item = item;
            entity.Value = item.Value;
            entity.ExpiredTime = DateTime.Now.AddSeconds(entity.ExpiredSeconds);
            cacheItemDic[item.Key] = entity;
            return true;
        }
    }
}
