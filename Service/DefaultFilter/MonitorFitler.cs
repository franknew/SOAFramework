using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Filter
{
    public class MonitorFitler : BaseFilter
    {
        private MonitorWatcher watcher = new MonitorWatcher();

        public MonitorFitler()
        {
            watcher.AddNoticer(new LogNoticer());
            watcher.AddNoticer(new FormNoticer());
        }

        public override bool OnActionExecuting(ActionContext context)
        {
            watcher.AddMesssage(new CacheMessage { Message = "请求方法：" + context.Router.TypeName + "." + context.Router.Action });
            return true;
        }
    }
}
