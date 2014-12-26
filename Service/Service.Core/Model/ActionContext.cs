using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SOAFramework.Service.Model
{
    public class ActionContext
    {
        public List<object> Parameters { get; set; }

        public RouterData Router { get; set; }

        public HttpContext Context { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public PerformanceContext PerformanceContext { get; set; }
    }
}
