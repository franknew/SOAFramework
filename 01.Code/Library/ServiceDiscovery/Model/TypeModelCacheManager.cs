using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class TypeModelCacheManager
    {
        const string _key = "__typeModel";

        public static TypeModel Get(string key)
        {
            CacheManager<TypeModel> manager = new CacheManager<TypeModel>(_key);
            return manager.GetItem(key);
        }

        public static void Set(string key, TypeModel t)
        {
            CacheManager<TypeModel> manager = new CacheManager<TypeModel>(_key);
            manager.SetItem(key, t);
        }
    }
}
