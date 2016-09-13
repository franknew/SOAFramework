using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    public interface IFilter
    {
        bool OnActionExecuting();
        bool OnActionExecuted();
        bool OnException();
    }
}
