using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Model;
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
using System.Xml;
using System.Xml.Linq;

namespace SOAFramework.Service.Server
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceLayer(Enabled = false)]
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
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteMethod(string typeName, string functionName
            , List<object> args)
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
            ServiceModel service = ServicePoolManager.GetItem<ServiceModel>(key);

            MethodInfo method = service.MethodInfo;
            if (method == null)
            {
                throw new Exception("方法不存在，错误的接口名或者方法！");
            }
            var instance = Activator.CreateInstance(method.DeclaringType);
            object[] parameters = null;
            if (args != null)
            {
                parameters = args.ToArray();
            }
            result = method.Invoke(instance, parameters);
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
            assmList.Add(Assembly.GetExecutingAssembly());
            assmList.Add(Assembly.GetCallingAssembly());
            foreach (string value in config.Values)
            {
                Assembly ass = Assembly.Load(value);
                if (ass == null)
                {
                    throw new Exception("业务层配置错误，无法加载相应的DLL：" + value);
                }
                if (assmList.Contains(ass))
                {
                    continue;
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
        /// <param name="typeName"></param>
        /// <param name="funcName"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IFilter FilterExecuting(List<IFilter> filterList, string typeName, string funcName, MethodInfo method,
            List<object> parameters)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="typeName"></param>
        /// <param name="funcName"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="ElapsedMilliseconds"></param>
        /// <returns></returns>
        public static IFilter FilterExecuted(List<IFilter> filterList, string typeName, string funcName, MethodInfo method,
            List<object> parameters, long ElapsedMilliseconds)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assmList"></param>
        private static void AddAssInCache(List<Assembly> assmList)
        {
            foreach (var ass in assmList)
            {
                //加载程序集注释文件
                FileInfo file = new FileInfo(ass.Location.Remove(ass.Location.LastIndexOf(".")) + ".xml");
                XElement document = null;
                if (file.Exists)
                {
                    document = XElement.Load(file.FullName);
                }
                Type[] types = ass.GetTypes();
                foreach (var type in types)
                {
                    ServiceLayer layer = type.GetCustomAttribute<ServiceLayer>();
                    if (layer != null && !layer.Enabled)
                    {
                        continue;
                    }
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    List<MethodInfo> list = methods.ToList().FindAll(t => !t.Name.StartsWith("get_") && !t.Name.StartsWith("set_"));
                    foreach (var method in list)
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
                        //获得方法注释信息
                        ServiceLayer lay = method.DeclaringType.GetCustomAttribute<ServiceLayer>();
                        ServiceInvoker attributeInvoker = method.GetCustomAttribute<ServiceInvoker>();
                        ServiceInfo info = null;
                        //如果类上或者方法上设置了隐藏发现，就不能通过这个方法显示出来
                        if ((lay == null || !lay.IsHiddenDiscovery) && (attributeInvoker == null || !attributeInvoker.IsHiddenDiscovery))
                        {
                            XElement methodElement = null;
                            if (document != null)
                            {
                                methodElement = (from d in document.Descendants("member")
                                                 where d.Attribute("name").Value.StartsWith("M:" + key)
                                                 select d).FirstOrDefault();
                            }
                            info = new ServiceInfo { InterfaceName = key };
                            if (methodElement != null)
                            {
                                string desc = methodElement.Element("summary").Value.Trim('\n', ' ');
                                string returndesc = methodElement.Element("returns").Value.Trim('\n', ' ');
                                if (!string.IsNullOrEmpty(desc))
                                {
                                    info.Description = desc;
                                }
                                if (!string.IsNullOrEmpty(returndesc))
                                {
                                    info.ReturnDesc = returndesc;
                                }
                            }
                            List<ServiceParameter> parameters = null;
                            ParameterInfo[] param = method.GetParameters();
                            if (param.Length > 0)
                            {
                                parameters = new List<ServiceParameter>();
                                foreach (ParameterInfo p in param)
                                {
                                    ServiceParameter sp = new ServiceParameter
                                    {
                                        Index = p.Position,
                                        Name = p.Name,
                                        TypeName = p.ParameterType.FullName,
                                    };
                                    if (methodElement != null)
                                    {
                                        string paramDesc = (from d in methodElement.Elements("param")
                                                            where d.Attribute("name").Value.Equals(p.Name)
                                                            select d.Value).FirstOrDefault();
                                        sp.Description = paramDesc;
                                    }
                                    parameters.Add(sp);
                                }
                            }
                        }
                        ServiceModel service = new ServiceModel { MethodInfo = method, ServiceInfo = info };
                        ServicePoolManager.AddItem(key, service);
                    }
                }
            }
        }

        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static List<object> ConvertParameters(MethodInfo method, Dictionary<string, string> args)
        {
            List<ParameterInfo> list = new List<ParameterInfo>();
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
                        listParameters.Add(JsonHelper.Deserialize(args[parameter.Name], parameter.ParameterType));
                    }
                }
            }
            return listParameters;
        }
    }
}
