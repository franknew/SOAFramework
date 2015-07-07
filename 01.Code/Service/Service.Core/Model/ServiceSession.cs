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
                    StackFrame frame = (new StackTrace()).GetFrames().FirstOrDefault(t=>t.GetMethod().DeclaringType.Equals(typeof(ServicePool)) && t.GetMethod().Name.ToLower().Equals("execute"));
                    if (frame == null)
                    {
                        return null;
                    }
                    string sessionid = frame.GetMethod().GetHashCode().ToString();
                    return ServicePool.Instance.Session[sessionid];
                }
            }
        }
    }
}
