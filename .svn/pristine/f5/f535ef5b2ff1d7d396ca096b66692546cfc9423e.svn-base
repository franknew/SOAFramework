
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class MonitorCache
    {
        private static readonly MonitorCache Instance = new MonitorCache();

        private Dictionary<string, List<CacheMessage>> messages = new Dictionary<string, List<CacheMessage>>();

        public static MonitorCache GetInstance()
        {
            return Instance;
        }

        public void PushMessage(CacheMessage message, CacheEnum cache)
        {
            List<CacheMessage> list = new List<CacheMessage>();
            if (messages.ContainsKey(cache.ToString()))
            {
                list = messages[cache.ToString()];
            }
            else
            {
                messages[cache.ToString()] = list;
            }
            list.Add(message);
        }

        public List<CacheMessage> PopMessages(CacheEnum cache)
        {
            List<CacheMessage> list = new List<CacheMessage>();
            if (messages.ContainsKey(cache.ToString()))
            {
                list = messages[cache.ToString()];
            }
            return list;
        }

        public void Clear(CacheEnum cache)
        {
            List<CacheMessage> list = new List<CacheMessage>();
            if (messages.ContainsKey(cache.ToString()))
            {
                list = messages[cache.ToString()];
            }
            list.Clear();
        }
    }

    public enum CacheEnum
    {
        FormMonitor,
        LogMonitor,
        DataBaseMonitor,
    }
}
