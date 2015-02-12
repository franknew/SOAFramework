using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Filter
{
    public class MessageWatcher
    {
        private List<IMessageNoticer> list = new List<IMessageNoticer>();

        public void AddNoticer(IMessageNoticer notice)
        {
            list.Add(notice);
        }

        public void AddMessage(CacheMessage message)
        {
            list.ForEach(t =>
            {
                t.Add(message);
            });
        }
    }
}
