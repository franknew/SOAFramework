using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SOAFramework.Service.Core.Model
{
    public class ActionContext
    {
        public ActionContext(string typeName, string actionName, MethodInfo method, int elapsedMilliseconds,
            Dictionary<string, object> parameters, ServerResponse response)
        {
            this.routerData.TypeName = typeName;
            this.routerData.Action = actionName;
            this.parameters = parameters;
            this.Response = response;
            this.Context = OperationContext.Current;
            this.MethodInfo = method;
            this.performanceContext.ElapsedMilliseconds = elapsedMilliseconds;
        }

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

        public OperationContext Context { get; set; }

        public MethodInfo MethodInfo { get; set; }

        private PerformanceContext performanceContext = new PerformanceContext();

        public PerformanceContext PerformanceContext
        {
            get { return performanceContext; }
            set { performanceContext = value; }
        }

        public ServerResponse Response { get; set; }

        public int Code { get; set; }
    }
}
