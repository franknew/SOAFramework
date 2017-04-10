using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public class Memcache : ICache
    {
        const string ConfigSection = "Memcached/MemcachedConfig";

        private int port = 11211;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private string zone = "default";

        public string Zone
        {
            get { return zone; }
            set { zone = value; }
        }
        private string userName = null;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string password = null;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string ip = null;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private MemcachedClient client = null;

        public Memcache()
        {
            client = new MemcachedClient(ConfigSection);
        }

        public bool AddItem(System.Runtime.Caching.CacheItem item, int seconds = -1)
        {
            if (seconds > 0)
            {
                TimeSpan ts = new TimeSpan(seconds * 1000);
                return client.Store(StoreMode.Add, item.Key, item.Value, ts);
            }
            else
            {
                return client.Store(StoreMode.Add, item.Key, item.Value);
            }
        }

        public System.Runtime.Caching.CacheItem GetItem(string key)
        {
            object value = client.Get(key);
            return new System.Runtime.Caching.CacheItem(key, value);
        }

        public bool DelItem(System.Runtime.Caching.CacheItem item)
        {
            return client.Remove(item.Key);
        }

        public bool DelItem(string key)
        {
            return client.Remove(key);
        }

        public List<System.Runtime.Caching.CacheItem> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(System.Runtime.Caching.CacheItem item)
        {
            return client.Store(StoreMode.Replace, item.Key, item.Value);
        }
    }
}
