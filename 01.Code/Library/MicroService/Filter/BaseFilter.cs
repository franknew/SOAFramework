using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class BaseFilter : Attribute, IFilter
    {
        public virtual bool OnActionExecuting()
        {
            return true;
        }

        public virtual bool OnActionExecuted()
        {
            return true;
        }

        public virtual bool OnException()
        {
            return true;
        }
    }
}
