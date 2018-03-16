using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public static class DomainExtension
    {
        /// <summary>
        /// 根据程序集名称从域中获得程序集对象
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(this AppDomain domain, string assemblyName)
        {
            var ass = domain.GetAssemblies().FirstOrDefault(t=>t.GetName().Name.ToLower().Equals(assemblyName.ToLower()));
            if (ass == null) ass = Assembly.Load(assemblyName);
            return ass;
        }
    }
}
