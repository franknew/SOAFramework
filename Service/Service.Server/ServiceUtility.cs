using SOAFramework.Library;
using SOAFramework.Service.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SOAFramework.Service.Server
{
    public class ServiceUtility
    {
        const string bussinessConfig = "soaConfigGroup/businessFileConfig";
        const string filterConfig = "soaConfigGroup/filterConfig";
        const string businessLayer = null;
        const string filterLayer = null;

        /// <summary>
        /// 通过反射执行缓存中的方法
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="functionName"></param>
        /// <param name="cache"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteMethod(string typeName, string functionName
            , Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new Exception("没有设置类名！");
            }
            if (string.IsNullOrEmpty(functionName))
            {
                throw new Exception("没有设置方法名！");
            }
            object result = null;

            string key = typeName + "." + functionName;

            MethodInfo method = ServicePoolManager.GetItem<MethodInfo>(key);
            if (method == null)
            {
                throw new Exception("方法不存在，错误的接口名或者方法！");
            }
            var instance = Activator.CreateInstance(method.DeclaringType);
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.AddRange(method.GetParameters());
            parameters.Sort((l, r) => l.Position - r.Position);
            List<object> listParameters = null;
            if (parameters != null && parameters.Count > 0)
            {
                listParameters = new List<object>();
                foreach (var parameter in parameters)
                {
                    if (args.Keys.Contains(parameter.Name))
                    {
                        listParameters.Add(args[parameter.Name]);
                    }
                }
            }
            object[] paramArray = null;
            if (listParameters != null)
            {
                paramArray = listParameters.ToArray();
            }
            result = method.Invoke(instance, paramArray);
            return result;
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static void InitBusinessCache()
        {
            IDictionary config = ConfigurationManager.GetSection(bussinessConfig) as IDictionary;
            //设置业务层缓存
            if (config == null)
            {
                throw new Exception("配置错误，没有配置业务层dll！");
            }
            List<Assembly> assmList = new List<Assembly>();
            foreach (string value in config.Values)
            {
                Assembly ass = Assembly.Load(value);
                if (ass == null)
                {
                    throw new Exception("业务层配置错误，无法加载相应的DLL：" + value);
                }
                assmList.Add(ass);
            }

            AddAssInCache(assmList);
        }

        public static List<IFilter> InitFilterList()
        {
            IDictionary config = ConfigurationManager.GetSection(filterConfig) as IDictionary; 
            List<Assembly> assmList = new List<Assembly>();
            assmList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assmList.RemoveAll(t => t.FullName.StartsWith("System.") || t.FullName.StartsWith("Microsoft.") || t.FullName.Equals("System"));
            if (config != null)
            {
                foreach (string value in config.Values)
                {
                    Assembly ass = Assembly.Load(value);
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
                    if (type.GetInterface("IFilter") != null)
                    {
                        object instance = Activator.CreateInstance(type);
                        IFilter filter = instance as IFilter;
                        list.Add(filter);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns>运行失败的filter</returns>
        public static IFilter FilterExecuting(List<IFilter> filterList, string typeName, string funcName, MethodInfo method,
            Dictionary<string, object> parameters)
        {
            if (filterList != null)
            {
                foreach (IFilter filter in filterList)
                {
                    ActionContext context = new ActionContext
                    {
                        Context = HttpContext.Current,
                        Router = new RouterData
                        {
                            Action = funcName,
                            TypeName = typeName,
                        },
                        MethodInfo = method,
                        Parameters = parameters,
                    };
                    if (!filter.OnActionExecuting(context))
                    {
                        return filter;
                    }
                }
            }
            return null;
        }

        public static IFilter FilterExecuted(List<IFilter> filterList, string typeName, string funcName, MethodInfo method,
            Dictionary<string, object> parameters, long ElapsedMilliseconds)
        {
            if (filterList != null)
            {
                foreach (IFilter filter in filterList)
                {
                    ActionContext context = new ActionContext
                    {
                        Context = HttpContext.Current,
                        Router = new RouterData
                        {
                            Action = funcName,
                            TypeName = typeName,
                        },
                        MethodInfo = method,
                        Parameters = parameters,
                        PerformanceContext = new PerformanceContext
                        {
                            ElapsedMilliseconds = ElapsedMilliseconds,
                        },
                    };
                    if (!filter.OnActionExecuted(context))
                    {
                        return filter;
                    }
                }

            }
            return null;
        }

        private static void AddAssInCache(List<Assembly> assmList)
        {
            foreach (var ass in assmList)
            {
                Type[] types = ass.GetTypes();
                foreach (var type in types)
                {
                    ServiceLayer layer = type.GetCustomAttribute<ServiceLayer>();
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    if (layer != null && !layer.Enabled)
                    {
                        continue;
                    }
                    foreach (var method in methods)
                    {
                        string key = type.FullName + "." + method.Name;
                        if (layer == null)
                        {
                            ServiceInvoker attribute = method.GetCustomAttribute<ServiceInvoker>(true);
                            if (attribute != null)
                            {
                                if (!attribute.Enabled)
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(attribute.InterfaceName))
                                {
                                    key = attribute.InterfaceName;
                                }
                            }
                        }
                        ServicePoolManager.AddItem(key, method);
                    }
                }
            }
            Type defaultService = typeof(DefaultService);
            MethodInfo[] allMethods = defaultService.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var method in allMethods)
            {
                string key = defaultService.FullName + "." + method.Name;
                ServicePoolManager.AddItem(key, method);
            }
        }
    }
}
