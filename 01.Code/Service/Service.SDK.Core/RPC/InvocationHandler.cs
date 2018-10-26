using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Service.SDK.Core
{
    public interface InvocationHandler
    {
        object Invoke(int rid, string name, InvocationParameter[] args);
    }

    public class InvocationParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
