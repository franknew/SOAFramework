using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class AssemblyResolver
    {
        private Assembly _ass;

        public AssemblyResolver(Assembly ass)
        {
            _ass = ass;
        }

        public List<ServiceModel> Resolve(AttributeTargets target, Func<Type, bool> validate)
        {
            ResolverHelper.LoadDescription(_ass);
            List<ServiceModel> services = new List<ServiceModel>();
            Type[] types = null;
            try
            {
                types = _ass.GetTypes();
            }
            catch { }
            if (types == null) return services;
            foreach (var type in types)
            {
                if (type.FullName.Equals("System") || type.FullName.StartsWith("System.")) continue;
                bool forceResolve = false;
                if (target == AttributeTargets.Class) if (validate != null && validate.Invoke(type)) forceResolve = true;
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_")) continue;
                    ServiceModel service = null;
                    if (forceResolve) service = new ServiceModel(method);
                    else if (validate.Invoke(method.DeclaringType)) service = new ServiceModel(method);
                    if (service != null) services.Add(service);
                }
            }
            return services;
        }

    }
}
