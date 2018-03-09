using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class ModelFactory
    {
        public static TypeModel CreateTypeModel(Type type)
        {
            if (type == null) return null;
            var result = type.ToTypeModel();
            if (!result.IsArray && result.IsClass)
            {
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties != null)
                {
                    foreach (var p in properties)
                    {
                        TypeModel model = p.PropertyType.ToTypeModel();
                        model.MemberName = p.Name;
                        var desc = ResolverHelper.GetMemberFromCache(p.DeclaringType, t =>
                        {
                            return t.Name.StartsWith("P:" + p.DeclaringType.FullName + "." + p.Name);
                        });
                        model.Description = desc?.Summary?.Trim();
                        result.Properties.Add(model);
                    }
                }
            }
            return result;
        }
    }
}
