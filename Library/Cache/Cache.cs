
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class MonitorCache
    {
        private static readonly MonitorCache Instance = new MonitorCache();

        private List<CacheMessage> messages = new List<CacheMessage>();

        public static MonitorCache GetInstance()
        {
            return Instance;
        }

        public void PushMessage(CacheMessage message)
        {
            messages.Add(message);
        }

        public List<CacheMessage> PopMessages()
        {
            return messages;
        }

        public void Clear()
        {
            messages.Clear();
        }
    }

}
