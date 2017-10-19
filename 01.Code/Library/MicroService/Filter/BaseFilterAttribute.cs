using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class BaseFilterAttribute : Attribute, IFilterAttribute
    {
        public virtual HttpFilterResult OnActionExecuting(HttpServerContext context)
        {
            return new HttpFilterResult { };
        }

        public virtual HttpFilterResult OnActionExecuted(HttpServerContext context)
        {
            return new HttpFilterResult { };
        }

        public virtual HttpFilterResult OnException(HttpServerContext context, Exception ex)
        {
            return new HttpFilterResult { };
        }

        public int Index { get; set; }
    }
}
