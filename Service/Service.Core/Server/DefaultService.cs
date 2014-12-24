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
        /// <returns></returns>
        public List<ServiceModel> DiscoverService()
        {
            List<ServiceModel> list = null;
            List<MethodInfo> methodList = ServicePoolManager.GetAllItems<MethodInfo>();
            if (methodList.Count > 0)
            {
                list = new List<ServiceModel>();
                foreach (MethodInfo method in methodList)
                {
                    ServiceModel model = new ServiceModel();
                    string name = method.DeclaringType.FullName + "." + method.Name;
                    string description = null;
                    ServiceLayer lay = method.DeclaringType.GetCustomAttribute<ServiceLayer>();
                    //如果类上设置了隐藏发现，就不能通过这个方法显示出来
                    if (lay != null && lay.IsHiddenDiscovery)
                    {
                        continue;
                    }
                    ServiceInvoker attribute = method.GetCustomAttribute<ServiceInvoker>();
                    if (attribute != null)
                    {
                        //如果方法上设置了隐藏发现，就不能通过这个方法显示出来
                        if (attribute.IsHiddenDiscovery)
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(attribute.InterfaceName))
                        {
                            name = attribute.InterfaceName;
                        }
                        description = attribute.Description;
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
                            parameters.Add(sp);
                        }
                    }
                    model.InterfaceName = name;
                    model.Parameters = parameters;
                    list.Add(model);
                }
            }
            return list;
        }

        #region big data test
        public List<ServiceModel> BigDataTest()
        {
            List<ServiceModel> list = new List<ServiceModel>();
            for (int i = 0; i < 100000; i++)
            {
                ServiceModel model = new ServiceModel
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
