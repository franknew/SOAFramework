using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SOAFramework.Service.Core.Model
{
    public class ActionContext
    {
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public Dictionary<string, object> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private RouterData routerData = new RouterData();

        public RouterData Router
        {
            get { return routerData; }
            set { routerData = value; }
        }

        public HttpContext Context { get; set; }

        public MethodInfo MethodInfo { get; set; }

        private PerformanceContext performanceContext = new PerformanceContext();

        public PerformanceContext PerformanceContext
        {
            get { return performanceContext; }
            set { performanceContext = value; }
        }

        public ServerResponse Response { get; set; }
    }
}
