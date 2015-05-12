using SOAFramework.Library;
using SOAFramework.Service.Core;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.ServiceModel.Web;
using SOAFramework.Service.Server;

namespace SOAFramework.Service.Core.Model
{
    [ServiceLayer(Enabled = false)]
    public class ServicePool
    {
        #region attributes
        private Dictionary<string, ServiceModel> _pool = new Dictionary<string, ServiceModel>();
        private Dictionary<string, DateTime> _AssWatcher = new Dictionary<string, DateTime>();
        private List<IFilter> _filterList = new List<IFilter>();
        private List<Assembly> _businessAssList = new List<Assembly>();
        private List<MachinePerformance> _performanceList = new List<MachinePerformance>();
        private SOAConfiguration _config = null;
        #endregion

        #region singleton
        private static ServicePool instance = new ServicePool();
        private readonly static object syncRoot = new Object();
        internal static ServicePool Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ServicePool();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region contructor

        public ServicePool()
        {
            _config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
        #endregion

        #region properties
        public SOAConfiguration Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public List<IFilter> GlobalFilter
        {
            get { return _filterList; }
        }

        public bool EnableConsoleMonitor
        {
            get;
            set;
        }
        #endregion

        #region action
        public void Init()
        {
            _filterList = GetGlobalFilters();
            _businessAssList = GetBusinessAssmeblyList();
            FillPool(_filterList, _businessAssList);
        }

        public void FillPool()
        {
            _config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            _filterList = GetGlobalFilters();
            _businessAssList = GetBusinessAssmeblyList();
            FillPool(_filterList, _businessAssList);
        }

        public void FillPool(List<IFilter> filterList, List<Assembly> assmList)
        {
            foreach (var ass in assmList)
            {
                FillPoolWithSingleAssembly(ass, filterList);
            }
        }
        public void FillPoolWithSingleAssembly(Assembly assmbly, List<IFilter> filterList)
        {
            #region 判断监视缓存
            FileInfo assFile = new FileInfo(assmbly.Location);
            if (_AssWatcher.ContainsKey(assmbly.FullName))
            {
                if (assFile.LastWriteTime <= _AssWatcher[assmbly.FullName])
                {
                    return;
                }
                else
                {
                    _AssWatcher[assmbly.FullName] = assFile.LastWriteTime;
                }
            }
            else
            {
                _AssWatcher[assmbly.FullName] = assFile.LastWriteTime;
            }
            #endregion

            IAnalyzer analyzer = new ServiceAnalyzer(assmbly);
            analyzer.AnalyzeService(_pool, filterList);
        }

        public ServiceModel GetServiceModel(string interfaceName)
        {
            ServiceModel service = null;
            if (_pool.ContainsKey(interfaceName))
            {
                service = _pool[interfaceName];
            }
            return service;
        }

        public List<ServiceModel> GetAllServiceModel()
        {
            List<ServiceModel> list = new List<ServiceModel>();
            foreach (var key in _pool.Keys)
            {
                list.Add(_pool[key]);
            }
            return list;
        }

        public string GetMinCpuDispatcher()
        {
            MachinePerformance performance = null;
            foreach (var m in _performanceList)
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

        public void RemoveDispatcher(string url)
        {
            foreach (var m in _performanceList)
            {
                if (m.Url.ToLower().Equals(url.ToLower()))
                {
                    _performanceList.Remove(m);
                    break;
                }
            }
        }

        public void RegisterDispatcher(string url, float cpu)
        {
            MachinePerformance m = _performanceList.Find(t => t.Url == url);
            if (m == null)
            {
                m = new MachinePerformance
                {
                    Url = url,
                    CpuRate = cpu,
                };
                _performanceList.Add(m);
            }
            else
            {
                m.CpuRate = cpu;
            }
        }

        public float GetCpuRate()
        {
            Performance p = new Performance();
            p.GetCurrentCpuUsage();
            Thread.Sleep(100);
            return p.GetCurrentCpuUsage();
        }

        public object InvokeInterface(string typeName, string functionName, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new Exception("没有设置类名！");
            }
            if (string.IsNullOrEmpty(functionName))
            {
                throw new Exception("没有设置方法名！");
            }
            string interfaceName = typeName + "." + functionName;
            return InvokeInterface(interfaceName, args);
        }

        public object InvokeInterface(string interfaceName, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(interfaceName))
            {
                throw new Exception("没有设置接口名！");
            }
            object result = null;

            ServiceModel service = GetServiceModel(interfaceName);
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

        public Stream Execute(string typeName, string functionName, Dictionary<string, string> args)
        {
            //执行方法
            ServerResponse response = new ServerResponse();
            Stopwatch watch = new Stopwatch();
            Stopwatch allWatch = new Stopwatch();
            allWatch.Start();
            string json = "";
            string interfaceName = GetIntefaceName(typeName, functionName);
            try
            {
                #region 准备工作
                ServiceModel service = GetServiceModel(interfaceName);
                MethodInfo method = null;
                if (service != null)
                {
                    method = service.MethodInfo;
                }
                //如果找不到方法重新加载配置的DLL
                else
                {
                    ServicePool.Instance.FillPool();
                    service = GetServiceModel(interfaceName);
                    if (service != null)
                    {
                        method = service.MethodInfo;
                    }
                }
                //如果再找不到方法，说明没有配置
                if (method == null)
                {
                    throw new Exception("未能找到接口：" + interfaceName + "！");
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
                        object result = InvokeInterface(interfaceName, parsedArgs);
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
            if (EnableConsoleMonitor)
            {
                Console.WriteLine("{0} -- 耗时：{1}", interfaceName, allWatch.ElapsedMilliseconds);
            }
            #endregion
            return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
        }

        public string GetIntefaceName(string typeName, string functionName)
        {
            return typeName + "." + functionName;
        }
        #endregion

        #region helper method
        /// <summary>
        /// 获得公用的filter
        /// </summary>
        /// <returns></returns>
        private List<IFilter> GetGlobalFilters()
        {
            List<Assembly> assmList = new List<Assembly>();
            assmList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assmList.RemoveAll(t => t.FullName.StartsWith("System.") || t.FullName.StartsWith("Microsoft.") || t.FullName.Equals("System"));
            if (_config != null)
            {
                foreach (var value in _config.SOAConfig.FilterConfigSection.Configs)
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
                IAnalyzer analyzer = new ServiceAnalyzer(ass);
                list.AddRange(analyzer.AnalyzeFilter());
            }

            return list;
        }
        /// <summary>
        /// 获得配置中的业务程序集
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetBussinessConfigAss()
        {
            //设置业务层缓存
            if (_config == null)
            {
                throw new Exception("配置错误，没有配置业务层dll！");
            }
            List<Assembly> assmList = new List<Assembly>();
            foreach (var value in _config.SOAConfig.FilterConfigSection.Configs)
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
        /// <summary>
        /// 获得所有业务程序集
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetBusinessAssmeblyList()
        {
            List<Assembly> assmList = GetBussinessConfigAss();
            assmList.Add(Assembly.GetCallingAssembly());
            assmList.Add(Assembly.GetExecutingAssembly());
            return assmList;
        }

        #endregion
    }
}
