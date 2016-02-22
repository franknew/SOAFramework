using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SOAFramework.Library;

namespace SOAFramework.Library.WeiXin
{
    public class WeiXinSDK
    {
        public T Execute<T>(IWeiXinRequest<T> request) where T : WeiXinBaseResponse
        {
            //如果没有token，先去获得token
            if (string.IsNullOrEmpty(request.AccessToken))
            {
                SignIn signin = new SignIn();
                string token = signin.Do();
                request.AccessToken = token;
                ApiHelper.SetTokenIntoCache(token);
            }
            string fullurl = UrlConfig.Url + request.GetApi();
            T t = default(T);
            var type = request.GetType();
            var querystringproperty = type.GetProperty("QueryString", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            object querystring = null;
            if (querystringproperty != null) querystring = querystringproperty.GetValue(request, null);
            switch (request.RequestType)
            {
                case RequestType.Get:
                    t = ApiHelper.Get<T>(request.AccessToken, fullurl, querystring);
                    break;
                case RequestType.Post:
                    object postdata = null;
                    var postdataproperty = type.GetProperty("PostData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    if (postdataproperty != null) postdata = postdataproperty.GetValue(request, null);
                    fullurl = HttpHelper.CombineUrl(fullurl, querystring);
                    t = ApiHelper.Post<T>(request.AccessToken, fullurl, postdata);
                    break;
            }
            switch (t.errcode)
            {
                //token过期
                case "40001":
                    SignIn signin = new SignIn();
                    string token = signin.Do();
                    request.AccessToken = token;
                    ApiHelper.SetTokenIntoCache(token);
                    t = this.Execute(request);
                    break;
                default:
                    if (t.errcode != "0") throw new Exception(t.errmsg);
                    break;
            }
            return t;
        }

        public static readonly WeiXinSDK Instance = new WeiXinSDK();
    }
}
