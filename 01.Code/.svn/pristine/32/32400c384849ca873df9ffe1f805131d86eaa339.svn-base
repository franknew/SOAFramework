using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOAFramework.Service.Core
{
    [ServiceLayer(Enabled = false)]
    public class ServiceAnalyzer : IAnalyzer
    {
        private Assembly _ass = null;

        public ServiceAnalyzer(Assembly assembly)
        {
            _ass = assembly;
        }

        public void AnalyzeService(IDictionary<string, Model.ServiceModel> inputDic, List<IFilter> filterList)
        {
            GetService(inputDic, filterList);
        }

        public List<Model.IFilter> AnalyzeFilter()
        {
            List<IFilter> list = new List<IFilter>();
            if (_ass != null)
            {
                Type[] types = null;
                types = _ass.GetTypes();
                if (types != null)
                {
                    foreach (var type in types)
                    {
                        if (type.GetInterface("IFilter") != null && type.GetInterface("INoneExecuteFilter") == null)
                        {
                            object instance = Activator.CreateInstance(type);
                            IFilter filter = instance as IFilter;
                            if (filter != null)
                            {
                                FilterAttribute attr = type.GetCustomAttribute<FilterAttribute>(true);
                                if (attr != null)
                                {
                                    filter.Index = attr.Index;
                                    filter.GlobalUse = attr.GlobalUse;
                                }
                                list.Add(filter);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public static void SetFilterOnService(List<Model.IFilter> filterList, Model.ServiceModel service)
        {
            List<IFilter> filterListCopy = filterList.ToList();
            List<IFilter> methodFilter = new List<IFilter>();
            methodFilter.AddRange(service.MethodInfo.DeclaringType.GetCustomAttributes<BaseFilter>(true));
            methodFilter.AddRange(service.MethodInfo.GetCustomAttributes<BaseFilter>(true));
            List<IFilter> noneExecFilter = methodFilter.FindAll(t => t is INoneExecuteFilter);
            //移除所有不执行标签
            filterListCopy.RemoveAll(t => t is INoneExecuteFilter);
            foreach (var filter in noneExecFilter)
            {
                filterListCopy.RemoveAll(t => t.GetType().IsInstanceOfType(filter));
            }
            service.FilterList = filterListCopy;
        }

        #region helper method


        private void GetServiceFromType(Type type, List<XElement> elementList, IDictionary<string, ServiceModel> serviceDic,
            List<IFilter> filterList)
        {
            if (type.IsInterface)
            {
                return;
            }
            ServiceLayer layer = type.GetCustomAttribute<ServiceLayer>(false);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            //筛选，去掉getter和setter还有构造函数
            List<MethodInfo> list = methods.ToList().FindAll(t => !t.Name.StartsWith("get_")
                && !t.Name.StartsWith("set_")
                && t.DeclaringType.Name.IndexOf("<>c__DisplayClass") == -1);
            string module = "";
            bool hiddenService = false;
            if (layer != null)
            {
                //如果类上设置不可用，该类中所有的方法不进入服务池
                if (!layer.Enabled)
                {
                    return;
                }
                hiddenService = layer.IsHiddenDiscovery;
                module = layer.Module;
            }
            foreach (var method in list)
            {
                string key = ServicePool.Instance.GetIntefaceName(type.FullName, method.Name);

                ServiceInfo info = GetServiceInfoFromMethodInfo(method, elementList, hiddenService);
                if (string.IsNullOrEmpty(info.Module))
                {
                    info.Module = module;
                }
                ServiceModel model = new ServiceModel { MethodInfo = method, ServiceInfo = info, FilterList = GetFilterFromMethod(filterList, method) };
                serviceDic[key] = model;
            }
        }

        private ServiceInfo GetServiceInfoFromMethodInfo(MethodInfo method, List<XElement> elementList, bool isHiddenDiscovery)
        {
            ServiceInvoker attribute = method.GetCustomAttribute<ServiceInvoker>(true);

            string key = ServicePool.Instance.GetIntefaceName(method.DeclaringType.FullName, method.Name); ;

            string module = "";
            if (attribute != null)
            {
                //如果方法上设置了不可用，那么该方法就不进入服务池
                if (!attribute.Enabled)
                {
                    return null;
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
            if (!isHiddenDiscovery && attribute != null && attribute.IsHiddenDiscovery)
            {
                isHiddenDiscovery = true;
            }
            string description = "";
            string returnDesc = "";
            List<ServiceParameter> parameters = null;
            XElement methodElement = null;
            if (elementList != null)
            {
                methodElement = elementList.Find(t => t.Attribute("name").Value.StartsWith("M:" + key));
            }

            #region 获得回参和注释
            if (methodElement != null)
            {
                description = methodElement.Element("summary") != null ? methodElement.Element("summary").Value.Trim('\n', ' ') : "";
                returnDesc = methodElement.Element("returns") != null ? methodElement.Element("returns").Value.Trim('\n', ' ') : "";
            }
            #endregion

            #region 处理参数信息
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
                            IsClass = IsCustomClassType(p.ParameterType),
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
            #endregion

            #region 设置serviceinfo
            ServiceInfo info = new ServiceInfo
            {
                InterfaceName = key,
                Parameters = parameters,
                Module = module,
                ReturnTypeInfo = new SOAFramework.Service.Core.Model.TypeInfo
                {
                    FullTypeName = method.ReturnType.FullName,
                    TypeName = GetTypeName(method.ReturnType),
                    IsClass = IsCustomClassType(method.ReturnType),
                    NameSpace = method.ReturnType.Namespace,
                },
                IsHidden = isHiddenDiscovery,
                Description = description.Trim(),
                ReturnDesc = returnDesc.Trim(),
                Assembly = method.DeclaringType.Assembly,
            };
            #endregion
            return info;
        }

        public string GetTypeName(Type t)
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

        /// <summary>
        /// 是否用户自定义类
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool IsCustomClassType(Type t)
        {
            return (!t.Namespace.StartsWith("System") || t.IsGenericType || t.IsArray);
        }

        /// <summary>
        /// 获得方法中所有的过滤器
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private List<IFilter> GetFilterFromMethod(List<IFilter> filterList, MethodInfo method)
        {
            List<IFilter> filterListCopy = filterList.ToList();
            List<IFilter> methodFilter = new List<IFilter>();
            methodFilter.AddRange(method.DeclaringType.GetCustomAttributes<BaseFilter>(true));
            methodFilter.AddRange(method.GetCustomAttributes<BaseFilter>(true));
            List<IFilter> noneExecFilter = methodFilter.FindAll(t => t is INoneExecuteFilter);
            //移除所有不执行标签
            filterListCopy.RemoveAll(t => t is INoneExecuteFilter);
            foreach (var filter in noneExecFilter)
            {
                filterListCopy.RemoveAll(t => t.GetType().IsInstanceOfType(filter));
            }
            return filterListCopy;
        }

        /// <summary>
        /// 设置服务信息到内存池中
        /// </summary>
        /// <param name="serviceDic"></param>
        /// <param name="filterList"></param>
        public void GetService(IDictionary<string, ServiceModel> serviceDic, List<IFilter> filterList)
        {
            Type[] types = null;
            types = _ass.GetTypes();
            if (types != null)
            {
                #region 处理程序集中的代码注释
                //去掉程序集后缀名，加上.xml为程序集注释的xml文件
                FileInfo file = new FileInfo(_ass.Location.Remove(_ass.Location.LastIndexOf(".")) + ".xml");
                List<XElement> elementList = null;
                if (file.Exists)
                {
                    elementList = XElement.Load(file.FullName).Descendants("member").ToList();
                }
                #endregion

                foreach (var type in types)
                {
                    GetServiceFromType(type, elementList, serviceDic, filterList);
                }
            }
        }
        #endregion
    }
}
