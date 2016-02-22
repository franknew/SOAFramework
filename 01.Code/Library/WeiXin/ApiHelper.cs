using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using SOAFramework.Library.Cache;

namespace SOAFramework.Library.WeiXin
{
    public class ApiHelper
    {
        private static ICache _cache = CacheFactory.Create();

        public static T Post<T>(string accesstoken, string url, object args, Dictionary<string, object> querystring = null)
        {
            if (string.IsNullOrEmpty(accesstoken)) throw new Exception("没有access_token");
            if (querystring == null) querystring = new Dictionary<string, object>();
            querystring["access_token"] = accesstoken;
            string fullurl = HttpHelper.CombineUrl(url, querystring);
            string json = JsonHelper.Serialize(args);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = HttpHelper.Post(fullurl, data);
            return JsonHelper.Deserialize<T>(result);
        }

        public static T Post<T>(string accesstoken, string url, Dictionary<string, object> args, Dictionary<string, object> querystring = null)
        {
            return Post<T>(accesstoken, url, args, querystring);
        }

        public static T Get<T>(string accesstoken, string url, Dictionary<string, object> querystring = null)
        {
            if (string.IsNullOrEmpty(accesstoken)) throw new Exception("没有access_token");
            if (querystring == null) querystring = new Dictionary<string, object>();
            querystring["access_token"] = accesstoken;
            string fullurl = HttpHelper.CombineUrl(url, querystring);
            string result = HttpHelper.Get(fullurl);
            return JsonHelper.Deserialize<T>(result);
        }

        public static T Get<T>(string accesstoken, string url, object querystring = null)
        {
            Dictionary<string, object> querystringdic = new Dictionary<string, object>();
            var properties = querystring.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Length > 0)
            {
                foreach (var p in properties)
                {
                    querystringdic[p.Name] = p.GetValue(querystring, null);
                }
            }
            return Get<T>(accesstoken, url, querystringdic);
        }

        public static string GetTokenFromCache()
        {
            var item = _cache.GetItem(UrlConfig.TokenCacheName);
            if (item == null) return null;
            return item.Value as string;
        }

        public static void SetTokenIntoCache(string token)
        {
            CacheItem item = new CacheItem(UrlConfig.TokenCacheName, token);
            _cache.AddItem(item, 7200);
        }
    }
}
