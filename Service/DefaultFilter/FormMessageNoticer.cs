using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Filter
{
    public class FormMessageNoticer : IMessageNoticer
    {
        public void Add(CacheMessage message)
        {
            MonitorCache.GetInstance().PushMessage(message, CacheEnum.FormMonitor);
        }

        public void Clear()
        {
            MonitorCache.GetInstance().Clear(CacheEnum.FormMonitor);
        }
    }
}
