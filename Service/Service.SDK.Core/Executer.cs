using SOAFramework.Library;
using SOAFramework.Service.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public class Executer
    {
        private string _url = ConfigurationManager.AppSettings["ServiceUrl"];

        public T Execute<T>(IRequest<T> request, string url = null) where T : BaseResponse
        {
            T t = default(T);
            string api = request.GetApi();
            if (string.IsNullOrEmpty(url))
            {
                url = _url;
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("没有设置服务url！");
            }
            string typeName = api.Remove(api.LastIndexOf("."));
            string actionName = api.Substring(api.LastIndexOf(".") + 1);
            string fullUrl = url.TrimEnd('/') + "/Execute/" + typeName + "/" + actionName;
            Type requestType = request.GetType();
            PropertyInfo[] properties = requestType.GetProperties();
            List<PostArgItem> args = new List<PostArgItem>();
            foreach (PropertyInfo pro in properties)
            {
                ArgMapping mapping = pro.GetCustomAttribute<ArgMapping>();
                string name = pro.Name;
                if (mapping != null && !string.IsNullOrEmpty(mapping.Mapping))
                {
                    name = mapping.Mapping;
                }
                args.Add(new PostArgItem { Key = name, Value = JsonHelper.Serialize(pro.GetValue(request)) });
            }
            string json = JsonHelper.Serialize(args);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string zippedResponse = HttpHelper.Post(fullUrl, data);
            string response = ZipHelper.UnZip(zippedResponse);
            BaseResponseShadow shadow = null;
            try
            {
                if (response.Contains("\"IsError\""))
                {
                    shadow = JsonHelper.Deserialize<BaseResponseShadow>(response);
                }
            }
            catch
            {
            }
            t = Activator.CreateInstance<T>();
            if (shadow == null)
            {
                PropertyInfo[] responseProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (responseProperties != null && responseProperties.Length > 0)
                {
                    object o = null;
                    try
                    {
                        o = JsonHelper.Deserialize(response, responseProperties[0].PropertyType);
                    }
                    catch
                    {
                    }
                    responseProperties[0].SetValue(t, o);
                }
            }
            else
            {
                t.SetValues(shadow.IsError, shadow.ErrorMessage, shadow.StackTrace);
            }
            t.SetBody(response);
            return t;
        }
    }
}
