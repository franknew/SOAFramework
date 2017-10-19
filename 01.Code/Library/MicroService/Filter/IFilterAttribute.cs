using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MicroService.Library
{
    public interface IFilterAttribute
    {
        HttpFilterResult OnActionExecuting(HttpServerContext context);
        HttpFilterResult OnActionExecuted(HttpServerContext context);
        HttpFilterResult OnException(HttpServerContext context, Exception ex);
        int Index { get; set; }

    }
}
