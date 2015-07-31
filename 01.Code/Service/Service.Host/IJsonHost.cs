using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SOAFramework.Service.Host
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IJsonHost”。
    [ServiceContract]
    public interface IJsonHost
    {
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, UriTemplate = "Execute/{typeName}/{functionName}", Method = "POST")]
        [OperationContract]
        Stream Execute(string typeName, string functionName, Stream args);


        [WebInvoke(UriTemplate = "Ping", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        bool Ping();


        [WebInvoke(UriTemplate = "Download/{fileName}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Stream Download(string fileName);

        [WebInvoke(UriTemplate = "Upload/{fileName}", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Stream Upload(string fileName, string fileContent);

        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "RegisterDispatcher/{usage}", Method = "POST")]
        void RegisterDispatcher(string usage, string url);
    }
}
