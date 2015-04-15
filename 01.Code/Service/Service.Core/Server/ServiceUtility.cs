using SOAFramework.Library;
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
    [ServiceLayer(Enabled = false)]
    public class ServiceUtility
    {
        private static readonly Dictionary<string, DateTime> _dicWatcher = new Dictionary<string, DateTime>();
        private static readonly List<MachinePerformance> _listPerformance = new List<MachinePerformance>();
        private static SOAConfiguration config = null;
        internal static List<IFilter> filterList = new List<IFilter>();

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

        public static void Init()
        {
            filterList = InitFilterList();
            List<Assembly> assmList = GetBusinessAssmeblyList();

            AddAssInCache(assmList, filterList);
        }

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

            ServiceModel service = GetServiceModel(typeName, functionName);
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
        public static List<Assembly> GetBusinessAssmeblyList()
        {
            List<Assembly> assmList = GetBussinessConfigAss();
            assmList.Add(Assembly.GetCallingAssembly());
            assmList.Add(Assembly.GetExecutingAssembly());
            return assmList;
        }

        public static List<Assembly> GetBussinessConfigAss()
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

        public static List<IFilter> InitFilterList()
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
                    if (type.GetInterface("IFilter") != null)
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
        public static IFilter FilterExecuting(string typeName, string funcName, ServiceModel service,
            Dictionary<string, object> parameters)
        {
            if (filterList != null)
            {
                //执行公共的过滤器
                foreach (var filter in service.FilterList)
                {
                    ActionContext context = new ActionContext
                    {
                        Context = HttpContext.Current,
                        Router = new RouterData
                        {
                            Action = funcName,
                            TypeName = typeName,
                        },
                        MethodInfo = service.MethodInfo,
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

        public static IFilter FilterExecuted(string typeName, string funcName, ServiceModel servide,
            Dictionary<string, object> parameters, long ElapsedMilliseconds, ServerResponse response)
        {
            if (filterList != null)
            {
                //执行公共的过滤器
                foreach (var filter in servide.FilterList)
                {
                    ActionContext context = new ActionContext
                    {
                        Context = HttpContext.Current,
                        Router = new RouterData
                        {
                            Action = funcName,
                            TypeName = typeName,
                        },
                        MethodInfo = servide.MethodInfo,
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

        public static void AddAssInCache(List<Assembly> assmList, List<IFilter> filters)
        {
            foreach (var ass in assmList)
            {
                #region 判断监视缓存
                FileInfo assFile = new FileInfo(ass.Location);
                if (_dicWatcher.ContainsKey(ass.FullName))
                {
                    if (assFile.LastWriteTime <= _dicWatcher[ass.FullName])
                    {
                        continue;
                    }
                    else
                    {
                        _dicWatcher[ass.FullName] = assFile.LastWriteTime;
                    }
                }
                else
                {
                    _dicWatcher[ass.FullName] = assFile.LastWriteTime;
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
                                        TypeInfo = new SOAFramework.Service.Core.Model.TypeInfo
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
                                ReturnTypeInfo = new SOAFramework.Service.Core.Model.TypeInfo
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
                        AttatchFiltersToMethods(filters, model);
                        ServicePoolManager.AddItem(key, model);
                    }
                }
            }
        }

        public static void RegisterDispatcher(string url, float cpu)
        {
            MachinePerformance m = _listPerformance.Find(t => t.Url == url);
            if (m == null)
            {
                m = new MachinePerformance
                {
                    Url = url,
                    CpuRate = cpu,
                };
                _listPerformance.Add(m);
            }
            else
            {
                m.CpuRate = cpu;
            }
        }

        public static string GetMinCpuDispatcher()
        {
            MachinePerformance performance = null;
            foreach (var m in _listPerformance)
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

        public static Stream Execute(string typeName, string functionName, Dictionary<string, string> args, List<BaseFilter> filterList,
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
                    ServiceUtility.Init();
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
                IFilter failedFilter = ServiceUtility.FilterExecuting(typeName, functionName, service, parsedArgs);
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
                failedFilter = ServiceUtility.FilterExecuted(typeName, functionName, service, parsedArgs, watch.ElapsedMilliseconds, response);
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

        protected static ServiceModel GetServiceModel(string typeName, string functionName)
        {
            string key = typeName + "." + functionName;

            ServiceModel service = ServicePoolManager.GetItem<ServiceModel>(key);
            return service;
        }
    }
}
