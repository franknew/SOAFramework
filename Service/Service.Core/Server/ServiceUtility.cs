using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core.Model;
using SOAFramework.Service.Model;
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
    [ServiceLayer(Enabled = false)]
    public class ServiceUtility
    {
        const string bussinessConfig = "soaConfigGroup/businessFileConfig";
        const string filterConfig = "soaConfigGroup/filterConfig";
        const string businessLayer = null;
        const string filterLayer = null;

        private static readonly Dictionary<string, DateTime> dicWatcher = new Dictionary<string, DateTime>();
        private static readonly List<MachinePerformance> listPerformance = new List<MachinePerformance>();

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

            ServiceModel service = ServicePoolManager.GetItem<ServiceModel>(key);
            MethodInfo method = null;
            if (service != null)
            {
                method = service.MethodInfo;
            }
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
            List<Assembly> assmList = GetBussinessConfigAss();
            assmList.Add(Assembly.GetCallingAssembly());
            assmList.Add(Assembly.GetExecutingAssembly());

            AddAssInCache(assmList);
        }

        public static List<Assembly> GetBussinessConfigAss()
        {
            ConfigurationManager.RefreshSection(bussinessConfig);
            IDictionary config = ConfigurationManager.GetSection(bussinessConfig) as IDictionary;
            //设置业务层缓存
            if (config == null)
            {
                throw new Exception("配置错误，没有配置业务层dll！");
            }
            List<Assembly> assmList = new List<Assembly>();
            foreach (string value in config.Values)
            {
                Assembly ass = null;
                if (value.ToLower().EndsWith(".dll"))
                {
                    ass = Assembly.LoadFile(value);
                }
                else
                {
                    ass = Assembly.Load(value);
                }
                if (ass == null)
                {
                    throw new Exception("业务层配置错误，无法加载相应的DLL：" + value);
                }
                assmList.Add(ass);
            }
            return assmList;
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
            Dictionary<string, object> parameters, long ElapsedMilliseconds, ServerResponse response)
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
                        Response = response,
                    };
                    if (!filter.OnActionExecuted(context))
                    {
                        return filter;
                    }
                }

            }
            return null;
        }

        public static string GetTypeName(Type t)
        {
            string typeName = t.Name;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type TType = t.GetGenericArguments()[0];
                typeName = "List<" + TType.Name + ">";
            }
            else if (t.IsArray)
            {
                typeName = t.GetElementType().Name + "[]";
            }
            return typeName;
        }

        public static bool IsClassType(Type t)
        {
            return (!t.Namespace.StartsWith("System") || t.IsGenericType || t.IsArray);
        }

        public static void AddAssInCache(List<Assembly> assmList)
        {
            foreach (var ass in assmList)
            {
                #region 判断监视缓存
                FileInfo assFile = new FileInfo(ass.Location);
                if (dicWatcher.ContainsKey(ass.FullName))
                {
                    if (assFile.LastWriteTime <= dicWatcher[ass.FullName])
                    {
                        continue;
                    }
                    else
                    {
                        dicWatcher[ass.FullName] = assFile.LastWriteTime;
                    }
                }
                else
                {
                    dicWatcher[ass.FullName] = assFile.LastWriteTime;
                }
                #endregion

                Type[] types = ass.GetTypes();
                FileInfo file = new FileInfo(ass.Location.Remove(ass.Location.LastIndexOf(".")) + ".xml");
                List<XElement> elementList = null;
                if (file.Exists)
                {
                    elementList = XElement.Load(file.FullName).Descendants("member").ToList();
                }

                foreach (var type in types)
                {
                    ServiceLayer layer = type.GetCustomAttribute<ServiceLayer>();
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    List<MethodInfo> list = methods.ToList().FindAll(t => !t.Name.StartsWith("get_")
                        && !t.Name.StartsWith("set_")
                        && t.DeclaringType.Name.IndexOf("<>c__DisplayClass") == -1);
                    string module = "";
                    //如果类上设置不可用，该类中所有的方法不进入服务池
                    if (layer != null && !layer.Enabled)
                    {
                        continue;
                    }
                    //获得模块名
                    else if (layer != null && !string.IsNullOrEmpty(layer.Module))
                    {
                        module = layer.Module;
                    }
                    foreach (var method in list)
                    {
                        string key = type.FullName + "." + method.Name;
                        ServiceInfo info = null;
                        ServiceInvoker attribute = method.GetCustomAttribute<ServiceInvoker>(true);
                        if (attribute != null)
                        {
                            //如果方法上设置了不可用，那么该方法就不进入服务池
                            if (!attribute.Enabled)
                            {
                                continue;
                            }
                            //获得设置的接口名称
                            if (!string.IsNullOrEmpty(attribute.InterfaceName))
                            {
                                key = attribute.InterfaceName;
                            }
                            //获得模块名称
                            if (!string.IsNullOrEmpty(attribute.Module))
                            {
                                module = attribute.Module;
                            }
                        }
                        //如果方法或者类上设置了隐藏发现，就不能通过这个方法显示出来
                        if ((layer == null || !layer.IsHiddenDiscovery) && (attribute == null || !attribute.IsHiddenDiscovery))
                        {
                            string description = "";
                            string returnDesc = "";
                            List<ServiceParameter> parameters = null;
                            XElement methodElement = null;
                            if (elementList != null)
                            {
                                methodElement = elementList.Find(t => t.Attribute("name").Value.StartsWith("M:" + key));
                            }
                            if (methodElement != null)
                            {
                                description = methodElement.Element("summary").Value.Trim('\n', ' ');
                                returnDesc = methodElement.Element("returns").Value.Trim('\n', ' ');
                            }
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
                                        TypeInfo = new Model.TypeInfo
                                        {
                                            FullTypeName = p.ParameterType.FullName,
                                            TypeName = GetTypeName(p.ParameterType),
                                            IsClass = IsClassType(p.ParameterType),
                                            NameSpace = p.ParameterType.Namespace,
                                        },
                                    };
                                    XElement paramElement = null;
                                    if (methodElement != null)
                                    {
                                        paramElement = methodElement.Descendants("param").FirstOrDefault(t => t.Attribute("name").Value.Equals(p.Name));
                                    }
                                    if (paramElement != null && !string.IsNullOrEmpty(paramElement.Value))
                                    {
                                        sp.Description = paramElement.Value;
                                    }
                                    parameters.Add(sp);
                                }
                            }
                            info = new ServiceInfo
                            {
                                InterfaceName = key,
                                Parameters = parameters,
                                Module = module,
                                ReturnTypeInfo = new Model.TypeInfo
                                {
                                    FullTypeName = method.ReturnType.FullName,
                                    TypeName = GetTypeName(method.ReturnType),
                                    IsClass = IsClassType(method.ReturnType),
                                    NameSpace = method.ReturnType.Namespace,
                                },
                            };
                            if (!string.IsNullOrEmpty(description))
                            {
                                info.Description = description;
                            }
                            if (!string.IsNullOrEmpty(returnDesc))
                            {
                                info.ReturnDesc = returnDesc;
                            }
                        }

                        ServiceModel model = new ServiceModel { MethodInfo = method, ServiceInfo = info };
                        ServicePoolManager.AddItem(key, model);
                    }
                }
            }
        }

        public static void RegisterDispatcher(string url, float cpu)
        {
            MachinePerformance m = listPerformance.Find(t => t.Url == url);
            if (m == null)
            {
                m = new MachinePerformance
                {
                    Url = url,
                    CpuRate = cpu,
                };
                listPerformance.Add(m);
            }
            else
            {
                m.CpuRate = cpu;
            }
        }

        public static string GetMinCpuDispatcher()
        {
            MachinePerformance performance = null;
            foreach (var m in listPerformance)
            {
                if (performance == null || performance.CpuRate > m.CpuRate)
                {
                    performance = m;
                }
            }
            string url = "";
            if (performance != null)
            {
                url = performance.Url;
            }
            return url;
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

        public static Stream Execute(string typeName, string functionName, Dictionary<string, string> args, List<IFilter> filterList,
            bool enableConsoleMonitor)
        {

            //执行方法
            ServerResponse response = new ServerResponse();
            Stopwatch watch = new Stopwatch();
            Stopwatch allWatch = new Stopwatch();
            allWatch.Start();
            string json = "";
            try
            {
                #region 准备工作
                string methodFullName = typeName + "." + functionName;
                ServiceModel service = ServicePoolManager.GetItem<ServiceModel>(methodFullName);
                MethodInfo method = null;
                if (service != null)
                {
                    method = service.MethodInfo;
                }
                //如果找不到方法重新加载配置的DLL
                else
                {
                    ServiceUtility.InitBusinessCache();
                    service = ServicePoolManager.GetItem<ServiceModel>(methodFullName);
                    if (service != null)
                    {
                        method = service.MethodInfo;
                    }
                }
                //如果再找不到方法，说明没有配置
                if (method == null)
                {
                    throw new Exception("未能找到接口：" + methodFullName + "！");
                }
                Dictionary<string, object> parsedArgs = new Dictionary<string, object>();
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        if (args.ContainsKey(p.Name))
                        {
                            parsedArgs[p.Name] = JsonHelper.Deserialize(args[p.Name], p.ParameterType);
                        }
                    }
                }
                #endregion

                #region 执行前置filter
                IFilter failedFilter = ServiceUtility.FilterExecuting(filterList, typeName, functionName, method, parsedArgs);
                if (failedFilter != null)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                }
                #endregion

                #region 执行方法
                if (!response.IsError)
                {
                    try
                    {
                        watch.Start();
                        //执行方法
                        object result = ServiceUtility.ExecuteMethod(typeName, functionName, parsedArgs);
                        watch.Stop();
                        response.Data = result;
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    }
                    catch (Exception ex)
                    {
                        response.IsError = true;
                        response.ErrorMessage = ex.Message;
                        response.StackTrace = ex.StackTrace;
                    }
                }
                #endregion

                #region 执行后置filter
                failedFilter = ServiceUtility.FilterExecuted(filterList, typeName, functionName, method, parsedArgs, watch.ElapsedMilliseconds, response);
                if (failedFilter != null && !response.IsError)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                response.StackTrace = ex.StackTrace;
            }

            #region 处理结果
            //序列化对象成json
            if (response.IsError)
            {
                json = JsonHelper.Serialize(response);
            }
            else
            {
                json = JsonHelper.Serialize(response.Data);
            }
            //压缩json
            string zippedJson = ZipHelper.Zip(json);
            allWatch.Stop();
            if (enableConsoleMonitor)
            {
                Console.WriteLine("{0}.{1} -- 耗时：{2}", typeName, functionName, allWatch.ElapsedMilliseconds);
            }
            #endregion
            return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
        }
    }
}
