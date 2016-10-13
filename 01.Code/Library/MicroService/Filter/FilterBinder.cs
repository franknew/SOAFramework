using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class FilterBinder
    {
        const string sessionName = "__filters";

        public static void BindFilter<T>(Type type) where T : IFilter
        {
            var t = type.GetCustomAttributes(typeof(T), true);
            if (t == null || t.Length == 0) return;
            var filter = Activator.CreateInstance<T>();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var m in methods)
            {
                ServiceBinder.InitMethodHandler(sessionName, m, filter);
            }
        }

        public static void BindFilter()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        }
    }
}
