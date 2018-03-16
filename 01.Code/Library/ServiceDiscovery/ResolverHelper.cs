using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace SOAFramework.Library
{
    public class ResolverHelper
    {
        /// <summary>
        /// 解析域
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="target"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public static List<ServiceModel> ResolveDomain(AppDomain domain, AttributeTargets target, Func<Type, bool> validate)
        {
            var asses = domain.GetAssemblies();

            return ResolveAssemblyList(asses.ToList(), target, validate);
        }

        /// <summary>
        /// 解析程序集列表
        /// </summary>
        /// <param name="asses"></param>
        /// <param name="target"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public static List<ServiceModel> ResolveAssemblyList(IEnumerable<Assembly> asses, AttributeTargets target, Func<Type, bool> validate)
        {
            List<ServiceModel> services = new List<ServiceModel>();
            foreach (var ass in asses)
            {
                if (ass.FullName.StartsWith("System.")) continue;
                AssemblyResolver resolver = new AssemblyResolver(ass);
                var list = resolver.Resolve(target, validate);
                services.AddRange(list);
            }
            foreach (var service in services)
            {
                ServiceCacheManager.Set(service.ID, service);
            }
            return services;

        }

        /// <summary>
        /// 加载注释
        /// </summary>
        /// <param name="ass"></param>
        /// <returns></returns>
        public static DescriptionModel LoadDescription(Assembly ass)
        {
            DescriptionModel model = null;
            var path = ass.Location;
            var fullname = ass.FullName;
            var dllname = fullname.Split(',')[0].Trim();
            var descriptionFileName = string.Format("{0}.xml", path.Remove(path.Length - 4, 4));
            if (!File.Exists(descriptionFileName))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
                descriptionFileName = string.Format("{0}\\{1}.xml", path.TrimEnd('\\'), dllname);
                if (!File.Exists(descriptionFileName))
                {
                    path += "bin";
                    descriptionFileName = string.Format("{0}\\{1}.xml", path.TrimEnd('\\'), dllname);
                    if (!File.Exists(descriptionFileName)) return null;
                }
            }
            string xml = File.ReadAllText(descriptionFileName);
            try
            {
                model = XMLHelper.Deserialize<DescriptionModel>(xml);
                DescriptionModelCacheManager.Set(fullname, model);
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        /// <summary>
        /// 获得类型信息
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static TypeModel GetTypeFromCache(string typeName)
        {
            return TypeModelCacheManager.Get(typeName);
        }

        /// <summary>
        /// 获得类型信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TypeModel GetTypeFromCache(Type t)
        {
            var item = TypeModelCacheManager.Get(t.FullName);
            if (item != null) return item.Copy();
            else
            {
                item = new TypeModel();
                TypeModelCacheManager.Set(t.FullName, item);
                item = ModelFactory.CreateTypeModel(t);
                return item;
            }
        }

        /// <summary>
        /// 从缓存中获得描述信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DescriptionModel GetDescriptionFromCache(Type t)
        {
            var description = DescriptionModelCacheManager.Get(t.Assembly.FullName);
            if (description == null) description = ResolverHelper.LoadDescription(t.Assembly);

            return description;
        }

        /// <summary>
        /// 从缓存中获得成员信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static MemberDescription GetMemberFromCache(Type t, Func<MemberDescription, bool> func)
        {
            var description = GetDescriptionFromCache(t);
            return description?.Members?.FirstOrDefault(func);
        }

        /// <summary>
        /// 获得接口服务信息
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static ServiceModel GetServiceFromCache(string serviceId)
        {
            return ServiceCacheManager.Get(serviceId);
        }

        /// <summary>
        /// 获得已解析的所有接口
        /// </summary>
        /// <returns></returns>
        public static List<ServiceModel> GetServiceListFromCache()
        {
            return ServiceCacheManager.GetList();
        }
        
    }
}
