﻿using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace SOAFramework.Service.Server
{
    [ServiceLayer(Enabled = false, IsHiddenDiscovery = true)]
    public class ServiceUtility
    {
        private static SOAConfiguration config = null;

        public delegate bool FilterDelegate(ActionContext context);

        static ServiceUtility()
        {
            try
            {
                config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private static List<Assembly> GetBusinessAssmeblyList()
        {
            List<Assembly> assmList = GetBussinessConfigAss();
            assmList.Add(Assembly.GetCallingAssembly());
            assmList.Add(Assembly.GetExecutingAssembly());
            return assmList;
        }

        private static List<Assembly> GetBussinessConfigAss()
        {
            //设置业务层缓存
            if (config == null)
            {
                throw new Exception("配置错误，没有配置业务层dll！");
            }
            List<Assembly> assmList = new List<Assembly>();
            foreach (var value in config.SOAConfig.FilterConfigSection.Configs)
            {
                Assembly ass = null;
                if (value.Type.ToLower().EndsWith(".dll"))
                {
                    ass = Assembly.LoadFile(value.Type);
                }
                else
                {
                    ass = Assembly.Load(value.Type);
                }
                if (ass == null)
                {
                    throw new Exception("业务层配置错误，无法加载相应的DLL：" + value);
                }
                assmList.Add(ass);
            }
            return assmList;
        }

        private static List<IFilter> InitFilterList()
        {
            List<Assembly> assmList = new List<Assembly>();
            assmList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assmList.RemoveAll(t => t.FullName.StartsWith("System.") || t.FullName.StartsWith("Microsoft.") || t.FullName.Equals("System"));
            if (config != null)
            {
                foreach (var value in config.SOAConfig.FilterConfigSection.Configs)
                {
                    Assembly ass = null;
                    if (value.Type.ToLower().EndsWith(".dll"))
                    {
                        ass = Assembly.LoadFile(value.Type);
                    }
                    else
                    {
                        ass = Assembly.Load(value.Type);
                    }
                    if (ass == null)
                    {
                        continue;
                    }
                    if (!assmList.Contains(ass))
                    {
                        assmList.Add(ass);
                    }
                }
            }

            List<IFilter> list = new List<IFilter>();
            foreach (var ass in assmList)
            {
                Type[] types = ass.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterface("IFilter") != null && type.GetInterface("INoneExecuteFilter") == null)
                    {
                        object instance = Activator.CreateInstance(type);
                        IFilter filter = instance as IFilter;
                        if (filter.GlobalUse)
                        {
                            list.Add(filter);
                        }
                    }
                }
            }

            return list;
        }

        public static void AttatchFiltersToMethods(List<IFilter> filterList, ServiceModel service)
        {
            List<IFilter> filterListCopy = filterList.ToList();
            List<IFilter> methodFilter = new List<IFilter>();
            methodFilter.AddRange(service.MethodInfo.DeclaringType.GetCustomAttributes<BaseFilter>(true));
            methodFilter.AddRange(service.MethodInfo.GetCustomAttributes<BaseFilter>(true));
            service.FilterList = new List<IFilter>();
            List<IFilter> noneExecFilter = methodFilter.FindAll(t => t is INoneExecuteFilter);
            //移除所有不执行标签
            filterListCopy.RemoveAll(t => t is INoneExecuteFilter);
            foreach (var filter in noneExecFilter)
            {
                filterListCopy.RemoveAll(t => t.GetType().IsInstanceOfType(filter));
            }
            foreach (var filter in filterListCopy)
            {
                service.FilterList.Add(filter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns>运行失败的filter</returns>
        public static IFilter FilterExecuting(ServiceModel service, ActionContext context)
        {
            string typeName = service.ServiceInfo.InterfaceName.Substring(0, service.ServiceInfo.InterfaceName.LastIndexOf("."));
            string actionName = service.ServiceInfo.InterfaceName.Substring(service.ServiceInfo.InterfaceName.LastIndexOf(".") + 1);
            //执行公共的过滤器
            foreach (var filter in service.FilterList)
            {
                var result = InvokeFilter(service, filter, filter.OnActionExecuting, context);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static IFilter FilterExecuted(ServiceModel service, ServerResponse response, ActionContext context)
        {
            context = null;
            string typeName = service.ServiceInfo.InterfaceName.Substring(0, service.ServiceInfo.InterfaceName.LastIndexOf("."));
            string actionName = service.ServiceInfo.InterfaceName.Substring(service.ServiceInfo.InterfaceName.LastIndexOf(".") + 1);
            //执行公共的过滤器
            foreach (var filter in service.FilterList)
            {
                var result = InvokeFilter(service, filter, filter.OnActionExecuted, context);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static IFilter InvokeFilter(ServiceModel service, IFilter filter, FilterDelegate filterDelegate, ActionContext context)
        {
            if (filterDelegate == null)
            {
                return null;
            }
            var classAttr = service.MethodInfo.DeclaringType.GetCustomAttribute<IFilter>(true);
            var methodAttr = service.MethodInfo.GetCustomAttribute<IFilter>(true);
            if ((classAttr != null || methodAttr != null) && !(classAttr is INoneExecuteFilter) && !(methodAttr is INoneExecuteFilter))
            {
                if (!filterDelegate.Invoke(context))
                {
                    return filter;
                }
            }
            return null;
        }

        public static string GetTypeName(Type t)
        {
            string typeName = t.Name;
            Type nullableType = Nullable.GetUnderlyingType(t);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type TType = t.GetGenericArguments()[0];
                typeName = "List<" + TType.Name + ">";
            }
            else if (t.IsArray)
            {
                typeName = t.GetElementType().Name + "[]";
            }
            else if (nullableType != null)
            {
                typeName = nullableType.Name;
            }
            return typeName;
        }

        public static bool IsClassType(Type t)
        {
            return ((!t.Namespace.StartsWith("System") || t.IsGenericType || t.IsArray) && !t.IsValueType);
        }



        public static float GetCpuRate()
        {
            Performance p = new Performance();
            p.GetCurrentCpuUsage();
            Thread.Sleep(100);
            return p.GetCurrentCpuUsage();
        }

        public static string GetCurrentEndPoint()
        {
            ServiceModelSectionGroup group = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            if (group != null)
            {
                return group.Services.Services[0].Endpoints[0].Address.AbsoluteUri;
            }
            else
            {
                return null;
            }
        }

    }
}
