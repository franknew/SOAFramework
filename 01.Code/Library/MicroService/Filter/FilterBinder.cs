﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AustinHarris.JsonRpc;

namespace MicroService.Library
{
    public class FilterBinder
    {
        public static void BindFilter(string sessionName, Type type)
        {
            var classFilters = type.GetCustomAttributes(typeof(IFilterAttribute), true).Cast<IFilterAttribute>().ToList();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var m in methods)
            {
                List<IFilterAttribute> allFilters = new List<IFilterAttribute>();
                List<IFilterAttribute> enabledFilters = new List<IFilterAttribute>();
                allFilters.AddRange(classFilters);
                var methodFilters = m.GetCustomAttributes(typeof(IFilterAttribute), true).Cast<IFilterAttribute>().ToList();
                allFilters.AddRange(methodFilters);
                if (allFilters.Count == 0) continue;
                foreach (var f in allFilters)
                {
                    //查找继承INoneExecutable的filter，以便后面不执行相应过滤器
                    var filter = methodFilters.Find(t => (f.GetType().BaseType.Equals(t.GetType())));
                    if (f is INoneExecutableFilterAttribute && filter != null) continue;
                    if (!enabledFilters.Contains(f)) enabledFilters.Add(f);
                }
                var handler = FilterBinder.InitMethodHandler(sessionName, m, enabledFilters);
            }
        }

        public static FilterHandler InitMethodHandler(string sessionID, MethodInfo method, List<IFilterAttribute> filters)
        {
            var handler = FilterHandler.GetSessionHandler(sessionID);
            if (filters == null || filters.Count == 0) return handler;
            string name = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            if (handler.MetaData.ContainsKey(name)) filters = handler.MetaData[name];
            else handler.MetaData[name] = filters;
            filters.Sort((l, r) =>
            {
                return l.Index - r.Index;
            });
            return handler;
        }

        public static void BindFilter(string sessionName = null, AppDomain domain = null, List<Assembly> assList = null)
        {
            if (domain == null) domain = AppDomain.CurrentDomain;
            if (assList == null) assList = domain.GetAssemblies().ToList();
            foreach (var ass in assList)
            {
                var types = ass.GetTypes();
                foreach (var type in types)
                {
                    BindFilter(sessionName, type);
                }
            }
        }

        public static void Clear(string sessionName = null)
        {
            FilterHandler.DestroySession(sessionName);
        }
    }
}