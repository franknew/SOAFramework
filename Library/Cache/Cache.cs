
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

        public void PushMessage(CacheMessage message, CacheEnum key)
        {
            List<CacheMessage> list = new List<CacheMessage>();
            if (messages.ContainsKey(key.ToString()))
            {
                list = messages[key.ToString()];
            }
            else
            {
                messages[key.ToString()] = list;
            }
            list.Add(message);
        }

        public List<CacheMessage> PopMessages(CacheEnum key)
        {
            List<CacheMessage> list = new List<CacheMessage>();
            if (messages.ContainsKey(key.ToString()))
            {
                list = messages[key.ToString()];
            }
            return list;
        }

        public void Clear(CacheEnum key)
        {
            if (messages.ContainsKey(key.ToString()))
            {
                List<CacheMessage> list = messages[key.ToString()];
                list.Clear();
            }
        }
    }

    public enum CacheEnum
    {
        FormMonitor,
        LogMonitor,
        DataBaseMonitor,
    }

}
