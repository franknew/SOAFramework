using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class ResolverHelper
    {
        public static List<ServiceModel> ResolveDomain(AppDomain domain, AttributeTargets target, Func<Type, bool> validate)
        {
            var asses = domain.GetAssemblies();

            return ResolveAssemblyList(asses.ToList(), target, validate);
        }

        public static List<ServiceModel> ResolveAssemblyList(List<Assembly> asses, AttributeTargets target, Func<Type, bool> validate)
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

        public static TypeModel GetTypeFromCache(Type t)
        {
            var item = TypeModelCacheManager.Get(t.FullName);
            if (item != null) return item.Copy();
            else
            {
                item = new TypeModel();
                TypeModelCacheManager.Set(t.FullName, item);
                item = new TypeModel(t);
                return item;
            }
        }

        public static DescriptionModel GetDescriptionFromCache(Type t)
        {
            var description = DescriptionModelCacheManager.Get(t.Assembly.FullName);
            if (description == null) description = ResolverHelper.LoadDescription(t.Assembly);

            return description;
        }

        public static MemberDescription GetMemberFromCache(Type t, Func<MemberDescription, bool> func)
        {
            var description = GetDescriptionFromCache(t);
            return description?.Members?.FirstOrDefault(func);
        }

        public static ServiceModel GetServiceFromCache(string serviceId)
        {
            return ServiceCacheManager.Get(serviceId);
        }

        public static List<ServiceModel> GetServiceListFromCache()
        {
            return ServiceCacheManager.GetList();
        }

        public static List<ServiceModel> ResolveWebApi(List<ServiceModel> services)
        {
            foreach (var service in services)
            {
                service.FullAction = service.Type.Substring(0, service.Type.LastIndexOf(".")) + "/" + service.Name;
            }
            return services;
        }
    }
}
