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
        private MessageWatcher watcher = new MessageWatcher();
        public MonitorFitler()
        {
            watcher.AddNoticer(new FormMessageNoticer());
            watcher.AddNoticer(new LogMessageNoticer());
        }

        public override bool OnActionExecuting(ActionContext context)
        {
            watcher.AddMessage(new CacheMessage { Message = "请求方法：" + context.Router.TypeName + "." + context.Router.Action });
            return true;
        }
    }
}
