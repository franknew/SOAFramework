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
        private static SOAConfiguration config = null;

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

            ServiceModel service = ServicePool.Instance.GetServiceModel(typeName + "." + functionName);
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
        public static IFilter FilterExecuting(string typeName, string funcName, ServiceModel service,
            Dictionary<string, object> parameters)
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

            return null;
        }

        public static IFilter FilterExecuted(string typeName, string funcName, ServiceModel service,
            Dictionary<string, object> parameters, long ElapsedMilliseconds, ServerResponse response)
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
                ServiceModel service = ServicePool.Instance.GetServiceModel(methodFullName);
                MethodInfo method = null;
                if (service != null)
                {
                    method = service.MethodInfo;
                }
                //如果找不到方法重新加载配置的DLL
                else
                {
                    ServicePool.Instance.FillPool();
                    service = ServicePool.Instance.GetServiceModel(methodFullName);
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
    }
}
