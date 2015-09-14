using SOAFramework.Library;
using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core.Model
{
    public class ServiceSession : IDisposable
    {
        private static object syncRoot = new object();

        public ServiceInfo Service { get; set; }

        public MethodInfo Method { get; set; }

        public ActionContext Context { get; set; }

        public void Dispose()
        {
            Context = null;
        }


        public static ServiceSession Current
        {
            get
            {
                lock (syncRoot)
                {
                    StackFrame frame = (new StackTrace()).GetFrames().FirstOrDefault(t=>t.GetMethod().DeclaringType.Equals(typeof(ServicePool)) &&
                    t.GetMethod().GetCustomAttribute<ExecuteAttribute>(false) != null);
                    if (frame == null)
                    {
                        return null;
                    }
                    string sessionid = frame.GetMethod().GetHashCode().ToString();
                    //MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "sessionid=" + sessionid }, SOAFramework.Library.CacheEnum.FormMonitor);
                    if (ServicePool.Instance.Session.ContainsKey(sessionid))
                    {
                        return ServicePool.Instance.Session[sessionid];
                    }
                    else
                    {
                        MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = frame.GetMethod().Name + " no session sessionid=" + sessionid }, SOAFramework.Library.CacheEnum.FormMonitor);
                        return null;
                    }
                    
                }
            }
        }
    }
}
