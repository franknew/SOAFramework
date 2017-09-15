using Newtonsoft.Json.Linq;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace SOAFramework.Service.SDK.Core
{
    public class Client
    {
        private string _url = ConfigurationManager.ConnectionStrings["ServiceUrl"]?.ConnectionString;

        public T Execute<T>(IRequest<T> request, string url = null, ContentTypeEnum type = ContentTypeEnum.UrlEncoded) where T : BaseResponse
        {
            T t = default(T);
            if (string.IsNullOrEmpty(url))
            {
                //生成完整的url
                url = GetRealUrl(request, url);
            }
            //访问服务
            string response = PostService(request, url, type);
            //通过服务器返回的json生成response对象
            t = GenerateResponse<T>(response);
            return t;
        }

        public object Execute(object request, Type responseType, string url = null, ContentTypeEnum type = ContentTypeEnum.UrlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                //生成完整的url
                url = GetRealUrlBase(request, url);
            }
            //访问服务
            string response = PostService(request, url, type);
            object t = GenerateResponse(response, responseType);
            return t;
        }

        #region helper method
        private string GetRealUrl<T>(IRequest<T> request, string serviceUrl) where T : BaseResponse
        {
            return GetRealUrlBase(request, serviceUrl);
        }

        private string GetRealUrlBase(object request, string serviceUrl)
        {
            dynamic dyrequest = request;
            string api = dyrequest.GetApi();
            if (string.IsNullOrEmpty(serviceUrl))
            {
                serviceUrl = _url;
            }
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new Exception("没有设置服务url！");
            }
            string fullUrl = "";
            if (api.LastIndexOf(".") < 0) fullUrl = serviceUrl.TrimEnd('/') + "/" + api.TrimStart('/');
            else
            {
                string typeName = api.Remove(api.LastIndexOf("."));
                string actionName = api.Substring(api.LastIndexOf(".") + 1);
                fullUrl = serviceUrl.TrimEnd('/') + "/" + typeName + "/" + actionName;
            }
            return fullUrl;
        }

        private string PostService<T>(IRequest<T> request, string fullUrl, ContentTypeEnum type) where T : BaseResponse
        {
            Type requestType = request.GetType();
            PropertyInfo[] properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, object> argdic = new Dictionary<string, object>();
            //反射获得请求属性
            foreach (PropertyInfo pro in properties)
            {
                if (pro.GetCustomAttributes(typeof(PostDataAttribute), true).Length > 0)
                {
                    object proValue = pro.GetValue(request, null);
                    var proProperties = pro.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo p in proProperties)
                    {
                        ArgMapping mapping = p.GetCustomAttributes(typeof(ArgMapping), true).FirstOrDefault() as ArgMapping;
                        string name = p.Name;
                        if (mapping != null && !string.IsNullOrEmpty(mapping.Mapping)) name = mapping.Mapping;
                        object value = p.GetValue(proValue, null);
                        argdic[name] = value;
                    }
                }
                else
                {
                    ArgMapping mapping = pro.GetCustomAttributes(typeof(ArgMapping), true).FirstOrDefault() as ArgMapping;
                    string name = pro.Name;
                    if (mapping != null && !string.IsNullOrEmpty(mapping.Mapping)) name = mapping.Mapping; 
                    object value = pro.GetValue(request, null);
                    argdic[name] = value;
                }
            }
            //格式化成post数据
            IPostDataFormatter fomatter = PostDataFormatterFactory.Create(type);
            string json = fomatter.Format(argdic);
            string typeString = ContentTypeConvert.ToTypeString(type);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string zippedResponse = HttpHelper.Post(fullUrl, data, contentType: typeString);
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

        private string PostService(object request, string fullUrl, ContentTypeEnum type)
        {
            Type requestType = request.GetType();
            PropertyInfo[] properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
            string typeString = ContentTypeConvert.ToTypeString(type);
            string zippedResponse = HttpHelper.Post(fullUrl, data, contentType: typeString);
            //string response = ZipHelper.UnZip(zippedResponse);
            string response = zippedResponse;

            //设置请求信息
            if (request.GetType().IsSubclassOf(typeof(BaseRequest<>)))
            {
                dynamic req = request;
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
            T t = (T)GenerateResponse(response, typeof(T));
            return t;
        }

        private object GenerateResponse(string response, Type responseType)
        {
            object t = Activator.CreateInstance(responseType);
            BaseResponse dyt = t as BaseResponse;
            BaseResponseShadow shadow = null;
            try
            {
                //如果报错生成一个Response
                if (response.Contains("\"IsError\""))
                {
                    try
                    {
                        //t = JsonHelper.Deserialize(response, responseType);
                        shadow = JsonHelper.Deserialize<BaseResponseShadow>(response);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(response, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                dyt.SetBody(response);
                dyt.SetValues(true, ex.Message, ex.StackTrace, 0);
                return t;
            }
            //null意味着没有报错
            if (shadow != null && !shadow.IsError)
            {
                //将返回的对象值设置到response的第一个属性上面
                PropertyInfo[] responseProperties = responseType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (responseProperties != null && responseProperties.Length > 0)
                {
                    BaseResponseShadow o = null;
                    try
                    {
                        o = JsonHelper.Deserialize<BaseResponseShadow>(response);
                        object data = null;
                        if (o.Data is JArray && responseProperties[0].PropertyType.Equals(typeof(DataTable)))
                        {
                            data = (o.Data as JArray).ToDataTable();
                        }
                        else if (o.Data is JObject)
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
                        if (data is object)
                        {
                            try
                            {
                                data = Convert.ChangeType(data, responseProperties[0].PropertyType);
                            }
                            catch
                            {
                                data = data.Clone(responseProperties[0].PropertyType);
                            }
                        }
                        //t.TrySetValue(responseProperties[0].Name, data);
                        responseProperties[0].SetValue(t, data, null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(response, ex);
                    }
                }
            }
            else
            {
                //否则设置错误信息
                dyt.SetValues(shadow.IsError, shadow?.ErrorMessage, shadow?.StackTrace, shadow.Code);
            }
            dyt.SetBody(response);
            return t;
        }
        #endregion
    }
}
