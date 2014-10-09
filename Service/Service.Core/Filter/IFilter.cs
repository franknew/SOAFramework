using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    public interface IFilter
    {
        bool OnActionExecuting(ActionContext context);

        bool OnActionExecuted(ActionContext context);

        string Message { get; set; }
    }

    public class BaseFilter : IFilter
    {
        public virtual bool OnActionExecuting(ActionContext context)
        {
            return true;
        }

        public virtual bool OnActionExecuted(ActionContext context)
        {
            return true;
        }

        public string Message { get; set; }
    }
}
