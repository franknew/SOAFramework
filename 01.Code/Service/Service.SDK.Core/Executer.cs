using Newtonsoft.Json.Linq;
using SOAFramework.Library;
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
        private string _url = ConfigurationManager.ConnectionStrings["ServiceUrl"].ConnectionString;

        public T Execute<T>(IRequest<T> request, string url = null, PostDataFomaterType type = PostDataFomaterType.Json) where T : BaseResponse
        {
            T t = default(T);
            if (string.IsNullOrEmpty(url))
            {
                //生成完整的url
                url = GetRealUrl(request, url);
            }
            //访问服务
            string response = PostSerivce(request, url, type);
            //通过服务器返回的json生成response对象
            t = GenerateResponse<T>(response);
            return t;
        }

        #region helper method
        private string GetRealUrl<T>(IRequest<T> request, string serviceUrl) where T : BaseResponse
        {
            string api = request.GetApi();
            if (string.IsNullOrEmpty(serviceUrl))
            {
                serviceUrl = _url;
            }
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new Exception("没有设置服务url！");
            }
            string typeName = api.Remove(api.LastIndexOf("."));
            string actionName = api.Substring(api.LastIndexOf(".") + 1);
            string fullUrl = serviceUrl.TrimEnd('/') + "/Execute/" + typeName + "/" + actionName;
            return fullUrl;
        }

        private string PostSerivce<T>(IRequest<T> request, string fullUrl, PostDataFomaterType type) where T : BaseResponse
        {
            Type requestType = request.GetType();
            PropertyInfo[] properties = requestType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, object> argdic = new Dictionary<string, object>();
            //反射获得请求属性
            foreach (PropertyInfo pro in properties)
            {
                ArgMapping mapping = pro.GetCustomAttributes(typeof(ArgMapping), true).FirstOrDefault() as ArgMapping;
                string name = pro.Name;
                if (mapping != null && !string.IsNullOrEmpty(mapping.Mapping))
                {
                    name = mapping.Mapping;
                }
                object value = pro.GetValue(request, null);
                argdic[name] = value;
            }
            //格式化成post数据
            IPostDataFormatter fomatter = PostDataFormatterFactory.Create(type);
            string json = fomatter.Format(argdic);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string zippedResponse = HttpHelper.Post(fullUrl, data);
            //string response = ZipHelper.UnZip(zippedResponse);
            string response = zippedResponse;

            //设置请求信息
            if (request is BaseRequest<T>)
            {
                var req = request as BaseRequest<T>;
                req.Body = new RequestBody
                {
                    PostData = json,
                    URL = fullUrl,
                };
            }
            return response;
        }

        private T GenerateResponse<T>(string response) where T : BaseResponse
        {
            T t = Activator.CreateInstance<T>();
            BaseResponseShadow shadow = null;
            try
            {
                //如果报错生成一个Response
                if (response.Contains("\"IsError\""))
                {
                    try
                    {
                        shadow = JsonHelper.Deserialize<BaseResponseShadow>(response);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(response, ex);
                    }
                }
            }
            catch
            {
            }
            //null意味着没有报错
            if (shadow != null && !shadow.IsError)
            {
                //将返回的对象值设置到response的第一个属性上面
                PropertyInfo[] responseProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (responseProperties != null && responseProperties.Length > 0)
                {
                    BaseResponseShadow o = null;
                    try
                    {
                        o = JsonHelper.Deserialize<BaseResponseShadow>(response);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(response, ex);
                    }
                    object data = null;
                    if (o.Data is JObject)
                    {
                        data = (o.Data as JObject).ToObject(responseProperties[0].PropertyType);
                    }
                    else if (o.Data is JArray)
                    {
                        data = (o.Data as JArray).ToListObject(responseProperties[0].PropertyType);
                    }
                    else
                    {
                        data = o.Data;
                    }
                    responseProperties[0].SetValue(t, data, null);
                }
            }
            else
            {
                //否则设置错误信息
                t.SetValues(shadow.IsError, shadow.ErrorMessage, shadow.StackTrace);
            }
            t.SetBody(response);
            return t;
        }
        #endregion
    }
}
