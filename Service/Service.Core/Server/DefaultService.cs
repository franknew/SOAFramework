using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core;
using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Server
{
    [ServiceKnownType(typeof(DefaultService))]
    [ServiceLayer]
    public class DefaultService
    {
        /// <summary>
        /// 获得所有服务类型
        /// </summary>
        /// <returns>服务对象列表</returns>
        public List<ServiceInfo> DiscoverService()
        {
            List<ServiceModel> methodList = ServicePoolManager.GetAllItems<ServiceModel>();
            List<ServiceInfo> list = (from s in methodList
                                      where s.ServiceInfo != null
                                      select s.ServiceInfo).ToList();
            return list;
        }

        /// <summary>
        /// 根据接口名称获得服务信息
        /// </summary>
        /// <param name="name">接口名称</param>
        /// <returns>服务对象</returns>
        public ServiceInfo DiscoverServiceByName(string name)
        {
            List<ServiceModel> methodList = ServicePoolManager.GetAllItems<ServiceModel>();
            ServiceInfo info = (from s in methodList
                                where s.ServiceInfo != null && s.ServiceInfo.InterfaceName.Equals(name)
                                select s.ServiceInfo).FirstOrDefault();
            return info;
        }

        /// <summary>
        /// 根据模块名称获得相关服务信息
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>服务列表</returns>
        public List<ServiceInfo> DiscoverServiceByModule(string moduleName)
        {
            List<ServiceInfo> list = new List<ServiceInfo>();
            List<ServiceModel> methodList = ServicePoolManager.GetAllItems<ServiceModel>();
            var query = from s in methodList
                        where s.ServiceInfo != null && s.ServiceInfo.Module.Equals(moduleName)
                        select s.ServiceInfo;
            list = query.ToList();
            return list;
        }

        #region big data test
        public List<ServiceInfo> BigDataTest()
        {
            List<ServiceInfo> list = new List<ServiceInfo>();
            for (int i = 0; i < 100000; i++)
            {
                ServiceInfo model = new ServiceInfo
                {
                    Description = "test" + i.ToString(),
                    InterfaceName = "name" + i.ToString(),
                };
                list.Add(model);
            }
            return list;
        }
        #endregion
    }
}
