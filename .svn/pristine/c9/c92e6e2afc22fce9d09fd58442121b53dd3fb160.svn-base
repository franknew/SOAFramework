using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Filter
{
    public class MonitorWatcher
    {
        private List<IMonitorNoticer> list = new List<IMonitorNoticer>();

        public void AddNoticer(IMonitorNoticer noticer)
        {
            list.Add(noticer);
        }

        public void AddMesssage(CacheMessage message)
        {
            list.ForEach(t =>
            {
                t.Add(message);
            });
        }

    }
}
