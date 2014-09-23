using SOAFramework.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Server
{
    public class Test
    {
        [ServiceInvoker]
        public string TestMethod(string str, string str1)
        {
            return "hell world! " + str;
        }
    }

    public class FilterTest : IFilter
    {
        public bool OnActionExecuting(ActionContext context)
        {
            object[] attr = context.MethodInfo.GetCustomAttributes(typeof(AuthAttr), true);
            if (attr == null || attr.Length == 0)
            {
                this.Message = "验证失败！";
                return false;
            }
            return true;
        }

        public bool OnActionExecuted(ActionContext context)
        {
            return true;
        }

        public string Message { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthAttr : Attribute
    {
    }
}
