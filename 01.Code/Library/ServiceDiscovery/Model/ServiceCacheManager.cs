using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class ServiceCacheManager
    {
        const string _key = "__serviceModel";

        public static ServiceModel Get(string key)
        {
            CacheManager<ServiceModel> manager = new CacheManager<ServiceModel>(_key);
            return manager.GetItem(key);
        }

        public static void Set(string key, ServiceModel t)
        {
            CacheManager<ServiceModel> manager = new CacheManager<ServiceModel>(_key);
            manager.SetItem(key, t);
        }

        public static List<ServiceModel> GetList()
        {
            CacheManager<ServiceModel> manager = new CacheManager<ServiceModel>(_key);
            return manager.GetList();
        }
    }
}