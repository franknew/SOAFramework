using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class DescriptionModelCacheManager
    {
        const string _key = "__descriptionModel";

        public static DescriptionModel Get(string key)
        {
            CacheManager<DescriptionModel> manager = new CacheManager<DescriptionModel>(_key);
            return manager.GetItem(key);
        }

        public static void Set(string key, DescriptionModel t)
        {
            CacheManager<DescriptionModel> manager = new CacheManager<DescriptionModel>(_key);
            manager.SetItem(key, t);
        }
    }
}
