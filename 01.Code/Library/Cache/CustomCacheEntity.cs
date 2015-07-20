using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public class CustomCacheEntity
    { 
        public CacheItem Item { get; set; }

        public string Key { get; set; }

        public object Value { get; set; }

        public int ExpiredSeconds { get; set; }

        public DateTime ExpiredTime { get; set; }
    }
}
