using RedisBoost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public class RedisCache : ICache
    {
        private IRedisClient _client;

        public RedisCache()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;
            _client = RedisClient.ConnectAsync(connectionstring).Result;
            _client.FlushDbAsync().Wait();
        }

        public bool AddItem(string key, object value, int seconds)
        {
            string json = JsonHelper.Serialize(value);
            _client.SetAsync(key, json).Wait();
            return true;
        }

        public bool DelItem(string key)
        {
            _client.DelAsync(key).Wait();
            return true;
        }

        public Dictionary<string, T> GetAllItems<T>()
        {
            throw new NotImplementedException();
        }

        public T GetItem<T>(string key)
        {
            T t = default(T);
            string json = _client.GetAsync(key).Result.As<string>();
            if (!string.IsNullOrEmpty(json)) t = JsonHelper.Deserialize<T>(json);
            return t;
        }

        public bool UpdateItem(string key, object value)
        {
            string json = JsonHelper.Serialize(value);
            _client.SetAsync(key, json).Wait();
            return true;
        }
    }
}
