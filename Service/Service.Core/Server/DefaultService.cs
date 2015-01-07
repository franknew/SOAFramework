using SOAFramework.Library;
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
        /// <returns></returns>
        public List<ServiceInfo> DiscoverService()
        {
            List<ServiceInfo> list = null;
            List<ServiceModel> methodList = ServicePoolManager.GetAllItems<ServiceModel>();
            if (methodList.Count > 0)
            {
                list = new List<ServiceInfo>();
                foreach (ServiceModel service in methodList)
                {
                    if (service.ServiceInfo != null)
                    {
                        list.Add(service.ServiceInfo);
                    }
                }
            }
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
