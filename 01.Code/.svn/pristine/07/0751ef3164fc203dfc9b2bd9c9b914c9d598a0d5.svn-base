using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroService.Library
{
    public class ServiceModelInfo
    {
        public string ServiceName { get; set; }

        public string TypeName
        {
            get
            {
                string typeName = "";
                if (!string.IsNullOrEmpty(ServiceName))
                {
                    int last = ServiceName.LastIndexOf(".");
                    typeName = ServiceName.Substring(0, last);
                }
                return typeName;
            }
        }

        public string MethodName
        {
            get
            {
                string methodName = "";
                if (!string.IsNullOrEmpty(ServiceName))
                {
                    int last = ServiceName.LastIndexOf(".");
                    methodName = ServiceName.Substring(last + 1, ServiceName.Length - last);
                }
                return methodName;
            }
        }

        public MethodInfo Method { get; set; }
    }
}
