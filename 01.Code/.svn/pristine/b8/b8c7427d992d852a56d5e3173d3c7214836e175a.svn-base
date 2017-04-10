using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core.Model;
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
            this.GlobalUse = true;
        }
        
        public override bool OnActionExecuted(ActionContext context)
        {
            if (context.Response.IsError)
            {
                watcher.AddMesssage(new CacheMessage { Message = "方法：" + context.Router.TypeName + "." + context.Router.Action + "发生错误,错误原因：" + context.Response.ErrorMessage + " 耗时：" + context.PerformanceContext.ElapsedMilliseconds.ToString() });
            }
            else
            {
                watcher.AddMesssage(new CacheMessage { Message = "请求方法：" + context.Router.TypeName + "." + context.Router.Action + " 耗时：" + context.PerformanceContext.ElapsedMilliseconds.ToString() });
            }
            return true;
        }
    }

    public class NoneExecMonitorFilter : MonitorFitler, INoneExecuteFilter
    {

    }
}
