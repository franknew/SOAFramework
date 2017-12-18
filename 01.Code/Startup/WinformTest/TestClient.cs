using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.Library;

namespace WinformTest
{
    public class TestClient: Client
    {
        //public override T Execute<T>(IRequest<T> request, string url = null, ContentTypeEnum type = ContentTypeEnum.Json)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        //生成完整的url
        //        url = GetRealUrlBase(request, url);
        //    }
        //    //访问服务
        //    string response = PostService(request, url, type);
        //    T t = GenerateResponse(response, typeof(T), false) as T;
        //    return t;
        //}
    }
}
