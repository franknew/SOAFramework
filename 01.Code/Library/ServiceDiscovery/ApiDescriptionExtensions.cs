using SOAFramework.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Description;

namespace SOAFramework.Library
{
    public static class ApiDescriptionExtensions
    {
        public static ServiceModel ToServiceModel(this ApiDescription api)
        {
            ServiceModel model = new ServiceModel();
            model.HttpMethod = api.HttpMethod.ToString();
            model.Route = api.RelativePath;
            model.Name = api.ActionDescriptor.ActionName;
            model.Return = api.ResponseDescription.DeclaredType.ToTypeModel();
            //if (model.Return != null && model.Return.IsArray)
            //{
            //    var properties = model.Return.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //    if (properties != null)
            //    {
            //        foreach (var p in properties)
            //        {
            //            TypeModel m = p.PropertyType.ToTypeModel();
            //            m.MemberName = p.Name;
            //            var desc = ResolverHelper.GetMemberFromCache(p.DeclaringType, t =>
            //            {
            //                return t.Name.StartsWith("P:" + p.DeclaringType.FullName + "." + p.Name);
            //            });
            //            m.Description = desc?.Summary?.Trim();
            //            model.Return.Properties.Add(m);
            //        }
            //    }
            //}
            model.Args = new List<TypeModel>();
            model.Description = api.Documentation;
            model.Category = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            model.FriendlyID = api.GetFriendlyId();
            if (api.ParameterDescriptions != null)
            {
                foreach (var p in api.ParameterDescriptions)
                {
                    var arg = p.ParameterDescriptor.ParameterType.ToTypeModel();
                    arg.MemberName = p.Name;
                    arg.Description = p.Documentation;
                    model.Args.Add(arg);
                }
            }
            return model;
        }

        public static ServiceModel ToServiceModel(this HelpPageApiModel api)
        {
            ServiceModel model = api.ApiDescription.ToServiceModel();
            return model;
        }
        

        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="ApiDescription"/>. E.g. "Get-Values-id_name" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="ApiDescription"/>.</param>
        /// <returns>The ID as a string.</returns>
        public static string GetFriendlyId(this ApiDescription description)
        {
            string path = description.RelativePath;
            string[] urlParts = path.Split('?');
            string localPath = urlParts[0];
            string queryKeyString = null;
            if (urlParts.Length > 1)
            {
                string query = urlParts[1];
                string[] queryKeys = HttpUtility.ParseQueryString(query).AllKeys;
                queryKeyString = String.Join("_", queryKeys);
            }

            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}",
                description.HttpMethod.Method,
                localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            if (queryKeyString != null)
            {
                friendlyPath.AppendFormat("_{0}", queryKeyString.Replace('.', '-'));
            }
            return friendlyPath.ToString();
        }
       
        public static TypeModel ToTypeModel(this Type t)
        {
            if (t == null) return null;
            TypeModel model = TypeModelCacheManager.Get(t.FullName);
            if (model != null) return model;
            model = t.ToTypeModelWithoutProperties();
            if (model.Properties.Count == 0)
            {
                var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties != null)
                {
                    foreach (var p in properties)
                    {
                        TypeModel m = p.PropertyType.ToTypeModelWithoutProperties();
                        m.MemberName = p.Name;
                        var desc = ResolverHelper.GetMemberFromCache(p.DeclaringType, x =>
                        {
                            return x.Name.StartsWith("P:" + p.DeclaringType.FullName + "." + p.Name);
                        });
                        m.Description = desc?.Summary?.Trim();
                        model.Properties.Add(m);
                    }
                }
            }
            if (model.IsClass) TypeModelCacheManager.Set(t.FullName, model);
            return model;
        }

        public static TypeModel ToTypeModelWithoutProperties(this Type t)
        {
            if (t == null) return null;
            var model = new TypeModel();
            model.Name = t.Name.Contains("`") ? t.Name.Remove(t.Name.LastIndexOf("`")) : t.Name ;
            model.FullName = t.FullName;
            model.GenericArguments = new List<TypeModel>();
            model.Properties = new List<TypeModel>();
            model.IsArray = t.IsArray;
            model.IsClass = true;
            model.Type = t;
            if (t.IsValueType || t.Equals(typeof(string)) || t.Equals(typeof(Type)))//handle string and value type
            {
                var nullableType = Nullable.GetUnderlyingType(t);
                //处理nullable值
                if (nullableType != null) return nullableType.ToTypeModelWithoutProperties();
                model.IsClass = false;
            }
            else if (t.GetInterface("IList") != null || t.GetInterface("IEnumerable") != null)
            {
                if (t.IsGenericType)
                {
                    var arguments = t.GetGenericArguments();
                    if (arguments != null)
                    {
                        foreach (var a in arguments)
                        {
                            var type = a;
                            //model = type.ToTypeModelWithoutProperties();
                            var typeModel = type.ToTypeModelWithoutProperties();
                            model.GenericArguments.Add(typeModel);
                            model.Type = t;
                        }
                    }
                }
                model.IsClass = false;
                model.IsArray = true;
            }
            else if (t.IsGenericType)//handle generic
            {
                    var arguments = t.GetGenericArguments();
                    if (arguments != null)
                    {
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            var type = arguments[i];
                            var arg = type.ToTypeModelWithoutProperties();
                            model.Type = type;
                            model.GenericArguments.Add(arg);
                        }
                    }
            }
            else if (t.IsArray)//handle array
            {
                var type = t.GetElementType();
                //model = type.ToTypeModelWithoutProperties();
                var typeModel = type.ToTypeModelWithoutProperties();
                model.GenericArguments.Add(typeModel);
                model.Type = t;
            }
            return model;
        }

        public static TypeModel ToTypeModel(this string fullTypeName)
        {
            var model = TypeModelCacheManager.Get(fullTypeName);
            if (model == null)
            {
                var type = Type.GetType(fullTypeName);
                if (type == null) type = TypePool.GetType(fullTypeName);
                if (type != null) model = type.ToTypeModel();
            }
            return model;
        }
    }
}
