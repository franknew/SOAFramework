using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SOAFramework.Service.SDK.Core
{
    public class HttpHandler : InvocationHandler
    {
        public object Invoke(int rid, string name, InvocationParameter[] args)
        {
            return name;
        }
    }
}
