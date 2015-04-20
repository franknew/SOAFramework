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
using SOAFramework.Library;
using System.Threading;

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
        private SOAConfiguration config = null;
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
            config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            _filterList = GetGlobalFilters();
            _businessAssList = GetBusinessAssmeblyList();
            FillPool(_filterList, _businessAssList);
        }
        #endregion

        #region properties
        public SOAConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        public List<IFilter> GlobalFilter
        {
            get { return _filterList; }
        }
        #endregion

        #region action
        public void Init()
        {

        }

        public void FillPool()
        {
            config = XMLHelper.DeserializeFromFile<SOAConfiguration>(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
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
