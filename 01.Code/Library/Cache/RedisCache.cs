using RedisBoost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text;

namespace SOAFramework.Library.Cache
{
    public class RedisCache : ICache
    {
        private static IRedisClient _client;
        private static string connectionString;
        public RedisCache()
        {

        }

        static RedisCache()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;
            //Connect();
        }

        public bool AddItem(string key, object value, int seconds)
        {
            string json = JsonHelper.Serialize(value);

            using (var client = CreateClient())
            {
                client.SetAsync(key, json).Wait();
            }
            return true;
        }

        public bool DelItem(string key)
        {
            using (var client = CreateClient())
            {
                client.DelAsync(key).Wait();
            }
            return true;
        }

        public Dictionary<string, T> GetAllItems<T>()
        {
            throw new NotImplementedException();
        }

        public T GetItem<T>(string key)
        {
            T t = default(T);
            string json = null;

            using (var client = CreateClient())
            {
                json = client.GetAsync(key).Result.As<string>();
            }
            if (!string.IsNullOrEmpty(json)) t = JsonHelper.Deserialize<T>(json);
            return t;
        }

        public bool UpdateItem(string key, object value)
        {
            string json = JsonHelper.Serialize(value);

            using (var client = CreateClient())
            {
                client.SetAsync(key, json).Wait();
            }
            return true;
        }

        public IRedisClient CreateClient()
        {
            var pool = RedisClient.CreateClientsPool();
            IRedisClient client = pool.CreateClientAsync(connectionString).Result;
            client.AuthAsync();
            return client;
        }

        private static void Connect()
        {
            _client = RedisClient.ConnectAsync(connectionString).Result;
        }

        private static void Disconnect()
        {
            _client.DisconnectAsync().Wait();
        }
    }
}
