using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Filter
{
    public interface IMonitorNoticer
    {
        void Add(CacheMessage message);
    }
}
