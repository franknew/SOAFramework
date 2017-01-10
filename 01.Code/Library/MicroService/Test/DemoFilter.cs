using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    public class DemoFilter : BaseFilterAttribute
    {
        public override HttpFilterResult OnActionExecuted(HttpServerContext context)
        {
            return base.OnActionExecuted(context);
        }

        public override HttpFilterResult OnActionExecuting(HttpServerContext context)
        {
            return base.OnActionExecuting(context);
        }
    }

    public class Demo2Filter : BaseFilterAttribute
    {
        
    }

    public class DemoNoneFilter : DemoFilter, INoneExecutableFilterAttribute
    { }

}
