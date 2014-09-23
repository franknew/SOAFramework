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
}
