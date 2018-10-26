using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Service.SDK.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RouteAttribute: Attribute
    {
        private string route;
        public RouteAttribute(string route = null)
        {
            this.route = route;
        }
    }
}
