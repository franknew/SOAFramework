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
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace SOAFramework.Service.Core.Model
{
    [ServiceLayer(Enabled = false)]
    public class ServicePool
    {
        #region attributes
        protected Dictionary<string, ServiceModel> _pool = new Dictionary<string, ServiceModel>();
        protected Dictionary<string, DateTime> _AssWatcher = new Dictionary<string, DateTime>();
        protected List<IFilter> _filterList = new List<IFilter>();
        protected List<Assembly> _businessAssList = new List<Assembly>();
        protected List<MachinePerformance> _performanceList = new List<MachinePerformance>();
        protected SOAConfiguration _config = null;
        protected Dictionary<string, ServiceSession> _session = new Dictionary<string, ServiceSession>();
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

        /// <summary>
        /// 开启性能监控
        /// </summary>
        public bool EnableConsoleMonitor
        {
            get;
            set;
        }

        private bool enableRegDispatcher = false;
        /// <summary>
        /// 开启注册分发起
        /// </summary>
        public bool EnableRegDispatcher
        {
            get { return enableRegDispatcher; }
            set { enableRegDispatcher = value; }
        }

        /// <summary>
        /// 分发服务器url
        /// </summary>
        public string DispatchServerUrl { get; set; }

        public Dictionary<string, ServiceSession> Session
        {
            get { return _session; }
        }

        private bool enableZippedResponse = false;

        public bool EnableZippedResponse
        {
            get { return enableZippedResponse; }
            set { enableZippedResponse = value; }
        }
        #endregion

        #region action
        public void Init()
        {
            InitFromConfig();
            _filterList = GetGlobalFilters();
            _businessAssList = GetBusinessAssmeblyList();
            FillPool(_filterList, _businessAssList);
        }

        public void FillPool()
        {
            try
            {
                _config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                _filterList = GetGlobalFilters();
                _businessAssList = GetBusinessAssmeblyList();
                FillPool(_filterList, _businessAssList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message + " stack trace:" + ex.StackTrace);
            }
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

        public Stream Execute(string typeName, string functionName, Dictionary<string, object> args)
        {
            //执行方法
            ServerResponse response = new ServerResponse();
            Stopwatch watch = new Stopwatch();
            Stopwatch allWatch = new Stopwatch();
            ActionContext context = null;
            string sessionid = (new StackFrame()).GetMethod().GetHashCode().ToString();
            allWatch.Start();
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

                context = new ActionContext(typeName, functionName, method, -1, args, null);
                ServiceSession session = new ServiceSession
                {
                    Context = context,
                    Method = method,
                    Service = service.ServiceInfo,
                };
                _session[sessionid] = session;
                #endregion

                #region 执行前置filter

                IFilter failedFilter = ServiceUtility.FilterExecuting(service, context);
                if (failedFilter != null)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                    response.Code = context.Code;
                }
                #endregion

                #region 执行方法
                if (!response.IsError)
                {
                    watch.Start();
                    //执行方法
                    object result = service.Invoke(args);
                    watch.Stop();
                    response.Data = result;
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                }
                #endregion

                #region 执行后置filter
                context.PerformanceContext.ElapsedMilliseconds = watch.ElapsedMilliseconds;
                context.Response = response;
                _session[sessionid] = session;
                failedFilter = ServiceUtility.FilterExecuted(service, response, context);
                if (failedFilter != null && !response.IsError)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                    response.Code = context.Code;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Exception exinner = ex;
                while (exinner.InnerException != null)
                {
                    exinner = exinner.InnerException;
                }
                response.IsError = true;
                response.ErrorMessage = exinner.Message;
                response.StackTrace = exinner.StackTrace;
            }

            #region 处理结果
            Stream stream = response.ToStream(enableZippedResponse);
            allWatch.Stop();
            if (EnableConsoleMonitor)
            {
                Console.WriteLine("{0} -- 耗时：{1}", interfaceName, allWatch.ElapsedMilliseconds);
            }
            _session[sessionid].Dispose();
            _session.Remove(sessionid);
            #endregion

            return stream;
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
            //assmList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //assmList.RemoveAll(t => t.FullName.StartsWith("System.") || t.FullName.StartsWith("Microsoft.") || t.FullName.Equals("System"));
            if (_config != null
                && _config.SOAConfig != null
                && _config.SOAConfig.FilterConfigSection != null
                && _config.SOAConfig.FilterConfigSection.Configs != null)
            {
                foreach (var value in _config.SOAConfig.FilterConfigSection.Configs)
                {
                    Assembly ass = null;
                    if (value.Type.ToLower().EndsWith(".dll"))
                    {
                        string fullPath = GetAssmStaticPath(value.Type);
                        ass = Assembly.LoadFile(fullPath);
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
                var filterList = analyzer.AnalyzeFilter();
                if (filterList != null && filterList.Count > 0)
                {
                    foreach (var filter in filterList)
                    {
                        list.Add(filter);
                    }
                }
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
            if (_config == null || _config.SOAConfig == null || _config.SOAConfig.BusinessConfigSection.Configs == null)
            {
                return new List<Assembly>();
            }
            List<Assembly> assmList = new List<Assembly>();
            foreach (var value in _config.SOAConfig.BusinessConfigSection.Configs)
            {
                Assembly ass = null;
                if (value.Type.ToLower().EndsWith(".dll"))
                {
                    string dllPath = GetAssmStaticPath(value.Type);
                    ass = Assembly.LoadFile(dllPath);
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

        private string GetAssmStaticPath(string path)
        {
            string dllPath = path;
            if (path.IndexOf("\\") == -1)
            {
                dllPath = AppDomain.CurrentDomain.BaseDirectory + path;
            }
            else
            {
                dllPath = path.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
            }
            if (!File.Exists(dllPath))
            {
                dllPath = dllPath.Insert(dllPath.LastIndexOf('\\'), "\\bin");
            }
            return dllPath;
        }

        private void InitFromConfig()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableConsoleMonitor"]))
            {
                if (ConfigurationManager.AppSettings["EnableConsoleMonitor"] == "1")
                {
                    this.EnableConsoleMonitor = true;
                }
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableRegDispatcher"]))
            {
                if (ConfigurationManager.AppSettings["EnableRegDispatcher"] == "1")
                {
                    this.EnableRegDispatcher = true;
                }
                else
                {
                    this.EnableRegDispatcher = false;
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DispatcherServerUrl"]))
            {
                this.DispatchServerUrl = ConfigurationManager.AppSettings["DispatcherServerUrl"];
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableZipResponse"])
                && ConfigurationManager.AppSettings["EnableZipResponse"].Equals("1"))
            {
                this.enableZippedResponse = true;
            }
        }
        #endregion
    }
}
