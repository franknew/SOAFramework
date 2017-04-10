using SOAFramework.Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SOAFramework.Service.Server
{
    [ServiceLayer(Enabled = false)]
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IJsonHost
    {
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Execute/{typeName}/{functionName}", Method = "POST")]
        [OperationContract]
        Stream Execute(string typeName, string functionName, Stream args);


        [WebInvoke(UriTemplate = "Ping", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Ping();


        [WebInvoke(UriTemplate = "Download/{fileName}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream Download(string fileName);

        [WebInvoke(UriTemplate = "Upload/{fileName}", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream Upload(string fileName, string fileContent);

        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "RegisterDispatcher/{usage}", Method = "POST")]
        [OperationContract]
        void RegisterDispatcher(string usage, string url);
    }
}
