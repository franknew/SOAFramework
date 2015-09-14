using SOAFramework.Library;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core.Model;
using SOAFramework.Service.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOAFramework.Service.Server
{
    [ServiceKnownType(typeof(DefaultService))]
    [ServiceLayer(Module = "System")]
    public class DefaultService
    {
        /// <summary>
        /// 获得所有服务类型
        /// </summary>
        /// <returns>服务对象列表</returns>
        public List<ServiceInfo> DiscoverService()
        {
            List<ServiceModel> methodList = ServicePool.Instance.GetAllServiceModel();
            List<ServiceInfo> list = (from s in methodList
                                      where !s.ServiceInfo.IsHidden
                                      select s.ServiceInfo).ToList();
            return list;
        }

        /// <summary>
        /// 根据接口名称获得服务信息
        /// </summary>
        /// <param name="name">接口名称</param>
        /// <returns>服务对象</returns>
        [NoneExecMonitorFilter]
        public ServiceInfo DiscoverServiceByName(string name)
        {
            ServiceModel service = ServicePool.Instance.GetServiceModel(name);
            if (service != null && !service.ServiceInfo.IsHidden)
            {
                return service.ServiceInfo;
            }
            return null;
        }

        /// <summary>
        /// 根据模块名称获得相关服务信息
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>服务列表</returns>
        public List<ServiceInfo> DiscoverServiceByModule(string moduleName)
        {
            List<ServiceInfo> list = new List<ServiceInfo>();
            string module = moduleName;
            if (moduleName == "Default")
            {
                module = "";
            }
            List<ServiceModel> methodList = ServicePool.Instance.GetAllServiceModel();
            var query = from s in methodList
                        where s.ServiceInfo != null && s.ServiceInfo.Module == module
                        select s.ServiceInfo;

            list = query.ToList();
            list = (from l in list
                    orderby l.InterfaceName
                    select l).ToList();
            return list;
        }

        /// <summary>
        /// 获得服务池内所有的模块名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetServiceModuleNames()
        {
            List<ServiceModel> methodList = ServicePool.Instance.GetAllServiceModel();
            List<string> list = (from s in methodList
                                 where !s.ServiceInfo.IsHidden && !string.IsNullOrEmpty(s.ServiceInfo.Module)
                                 select s.ServiceInfo.Module).Distinct().ToList();
            if (!list.Contains("Default"))
            {
                list.Insert(0, "Default");
            }
            list = (from l in list
                   orderby l
                   select l).ToList();
            return list;
        }

        /// <summary>
        /// 根据类型全名获得类型的结构
        /// </summary>
        /// <param name="fullTypeName">类型全名</param>
        /// <returns>类型描述对象</returns>
        public TypeDescription GetTypeDescription(string fullTypeName)
        {
            Type type = Type.GetType(fullTypeName);
            TypeDescription t = null;
            if (type == null)
            {
                List<Assembly> list = AppDomain.CurrentDomain.GetAssemblies().ToList().FindAll(p => !p.FullName.StartsWith("System"));
                foreach (var a in list)
                {
                    type = a.GetType(fullTypeName);
                    if (type != null)
                    {
                        break;
                    }
                }
            }
            if (type != null)
            {
                string dllfileName = type.Assembly.CodeBase;
                string xmlFileName = dllfileName.Remove(dllfileName.LastIndexOf("."), 4) + ".xml";
                xmlFileName = xmlFileName.Replace("file:///", "").Replace("/", @"\");
                List<XElement> elementList = null;
                if (File.Exists(xmlFileName))
                {
                    elementList = XElement.Load(xmlFileName).Descendants("member").ToList();
                    if (elementList != null)
                    {
                        elementList = elementList.FindAll(p => p.Attribute("name").Value.StartsWith("P:"));
                    }
                }
                Type nullableType = Nullable.GetUnderlyingType(type);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                else if (type.IsArray)
                {
                    type = type.GetElementType();
                }
                else if (nullableType != null)
                {
                    type = nullableType;
                }
                t = new TypeDescription();
                t.TypeInfo = new SOAFramework.Service.Core.Model.TypeInfo
                {
                    FullTypeName = type.FullName,
                    NameSpace = type.Namespace,
                    TypeName = ServiceUtility.GetTypeName(type),
                    IsClass = ServiceUtility.IsClassType(type),
                };
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties != null)
                {
                    t.Properties = new List<PropertyDescription>();
                    foreach (PropertyInfo p in properties)
                    {
                        string remark = "";
                        if (elementList != null)
                        {
                            var propertyElement = elementList.Find(x => x.Attribute("name").Value.EndsWith("." + p.DeclaringType.Name + "." + p.Name));
                            if (propertyElement != null)
                            {
                                remark = propertyElement.Element("summary").Value.ToString();
                            }
                        }
                        PropertyDescription pd = new PropertyDescription
                        {
                            PropertyName = p.Name,
                            PropertyTypeInfo = new SOAFramework.Service.Core.Model.TypeInfo
                            {
                                FullTypeName = p.PropertyType.FullName,
                                NameSpace = p.PropertyType.Namespace,
                                IsClass = ServiceUtility.IsClassType(p.PropertyType),
                                TypeName = ServiceUtility.GetTypeName(p.PropertyType),
                            },
                            Remark = remark,
                        };
                        Type element = type.GetElementType();
                        if (element != null)
                        {
                            pd.PropertyTypeInfo.ElementFullTypeName = element.FullName;
                        }
                        t.Properties.Add(pd);
                    }
                }
            }
            return t;
        }

        #region big data test
        /// <summary>
        /// 大数据测试，构造10万个对象返回
        /// </summary>
        /// <returns></returns>
        [ServiceInvoker(Module = "Test")]
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
