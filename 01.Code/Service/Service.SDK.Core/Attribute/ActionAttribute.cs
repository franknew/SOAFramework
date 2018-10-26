using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Service.SDK.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute: Attribute
    {
        public string action = null;
        public ActionAttribute(string action = null)
        {
            this.action = action;
        }
    }
}
