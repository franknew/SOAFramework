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

        public virtual T Execute<T>(IRequest<T> request, string url = null, HttpMethodEnum method = HttpMethodEnum.Post, ContentTypeEnum type = ContentTypeEnum.UrlEncoded) where T : BaseResponse
        {
            var t = (T)Execute(request, typeof(T), url, method, type);
            return t;
        }

        public virtual object Execute(object request, Type responseType, string url = null, HttpMethodEnum method = HttpMethodEnum.Post, ContentTypeEnum type = ContentTypeEnum.UrlEncoded, IServiceEncryptor encryptor = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                //生成完整的url
                url = GetRealUrlBase(request, url);
            }
            //访问服务
            string response = InvokeService(request, url, type, method);
            object t = GenerateResponse(response, responseType);
            return t;
        }

        #region helper method
        public string GetRealUrl<T>(IRequest<T> request, string serviceUrl) where T : BaseResponse
        {
            return GetRealUrlBase(request, serviceUrl);
        }

        protected string GetRealUrlBase(object request, string serviceUrl)
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
            string fullUrl = serviceUrl.TrimEnd('/') + "/" + api.TrimStart('/');
            //if (api.LastIndexOf(".") < 0) fullUrl = serviceUrl.TrimEnd('/') + "/" + api.TrimStart('/');
            //else
            //{
            //    string typeName = api.Remove(api.LastIndexOf("."));
            //    string actionName = api.Substring(api.LastIndexOf(".") + 1);
            //    fullUrl = serviceUrl.TrimEnd('/') + "/" + typeName + "/" + actionName;
            //}
            return fullUrl;
        }

        protected string InvokeService<T>(IRequest<T> request, string fullUrl, ContentTypeEnum type, HttpMethodEnum method = HttpMethodEnum.Post, IServiceEncryptor encryptor = null) where T : BaseResponse
        {
            object req = request;
            var response = InvokeService(req, fullUrl, type, method, encryptor);
            return response;
        }

        protected string InvokeService(object request, string fullUrl, ContentTypeEnum type, HttpMethodEnum method = HttpMethodEnum.Post, IServiceEncryptor encryptor = null)
        {
            Type requestType = request.GetType();
            PropertyInfo[] properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, object> argdic = new Dictionary<string, object>();
            IDictionary<string, string> headers = new Dictionary<string, string>();
            IDictionary<string, string> cookies = new Dictionary<string, string>();
            //反射获得请求属性
            foreach (PropertyInfo pro in properties)
            {
                bool isPostData = false;
                bool isHeaderValue = false;
                bool isCookieValue = false;
                isPostData = pro.GetCustomAttributes(typeof(PostDataAttribute), true).Length > 0;
                isHeaderValue = pro.GetCustomAttributes(typeof(HeaderValueAttribute), true).Length > 0;
                isCookieValue = pro.GetCustomAttributes(typeof(CookieValueAttribute), true).Length > 0;
                if (isPostData)
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
                else if (isHeaderValue)
                {
                    object proValue = pro.GetValue(request, null);
                    headers[pro.Name] = proValue?.ToString();
                }
                else if (isCookieValue)
                {
                    object proValue = pro.GetValue(request, null);
                    cookies[pro.Name] = proValue?.ToString();
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
            string response = "";
            var postdata = "";
            switch (method)
            {
                default:
                    //格式化成post数据
                    IPostDataFormatter fomatter = PostDataFormatterFactory.Create(type);
                    postdata = fomatter.Format(argdic);
                    if (encryptor != null) postdata = encryptor.Encrypt(postdata);
                    string typeString = ContentTypeConvert.ToTypeString(type); 
                    byte[] data = Encoding.UTF8.GetBytes(postdata);
                    response = HttpHelper.Post(fullUrl, data, contentType: typeString, header: headers, cookieDic: cookies);
                    break;
                case HttpMethodEnum.Get:
                    type = ContentTypeEnum.UrlEncoded;
                    postdata = fullUrl;
                    response = HttpHelper.Get(fullUrl, argdic, header: headers, cookieDic: cookies);
                    break;
            }

            //设置请求信息
            if (request.GetType().IsSubclassOf(typeof(BaseRequest<>)))
            {
                dynamic req = request;
                req.Body = new RequestBody
                {
                    PostData = postdata,
                    URL = fullUrl,
                };
            }
            return response;
        }

        protected T GenerateResponse<T>(string response, bool toProperty = true) where T : BaseResponse
        {
            T t = (T)GenerateResponse(response, typeof(T), toProperty);
            return t;
        }

        protected object GenerateResponse(string response, Type responseType, bool toProperty = true)
        {
            object t = Activator.CreateInstance(responseType);
            BaseResponse dyt = t as BaseResponse;
            BaseResponseShadow shadow = null;
            //如果报错生成一个Response
            if (response.Contains("\"IsError\""))
            {
                try
                {
                    //t = JsonHelper.Deserialize(response, responseType);
                    shadow = JsonHelper.Deserialize<BaseResponseShadow>(response);
                    //否则设置错误信息
                    dyt.SetValues(shadow.IsError, shadow.Code, shadow.Message, shadow.Exception);
                }
                catch (Exception ex)
                {
                    throw new Exception(response, ex);
                }
            }
            else
            {
                if (!toProperty)
                {
                    t = JsonHelper.Deserialize(response, responseType);
                }
                else
                {
                    //null意味着没有报错
                    if (shadow == null)
                    {
                        PropertyInfo[] responseProperties = responseType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        if (responseProperties != null && responseProperties.Length > 0)
                        {
                            PropertyInfo result = responseProperties[0];
                            //先查找有没有sdk result attribute的属性
                            foreach (var p in responseProperties)
                            {
                                var pro = p.GetCustomAttributes(typeof(SDKResultAttribute), true);
                                if (pro != null && pro.Length > 0)
                                {
                                    result = p;
                                    break;
                                }
                            }
                            //将返回的对象值设置到response的第一个属性上面
                            BaseResponseShadow o = null;
                            try
                            {
                                o = new BaseResponseShadow();
                                object data = JsonHelper.Deserialize(response, result.PropertyType);
                                if (data is object)
                                {
                                    try
                                    {
                                        data = Convert.ChangeType(data, result.PropertyType);
                                    }
                                    catch
                                    {
                                        data = data.Clone(result.PropertyType);
                                    }
                                }
                                //t.TrySetValue(responseProperties[0].Name, data);
                                result.SetValue(t, data, null);
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
                        dyt.SetValues(shadow.IsError, shadow.Code, shadow.Message, shadow.Exception);
                    }
                }
            }
            dyt.SetBody(response);
            return t;
        }
        #endregion
    }
}
