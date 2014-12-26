using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SOAFramework.Service.Server
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract(Namespace = "http://www.cnblogs.com/WindBlog/")]
    public interface IService
    {
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "{typeName}/{functionName}", Method = "POST")]
        [OperationContract]
        Stream Execute(string typeName, string functionName, Dictionary<string, string> args);

        [WebInvoke(UriTemplate = "Download/{fileName}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Stream Download(string fileName);

        [WebInvoke(UriTemplate = "Upload/{fileName}", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Stream Upload(string fileName, string fileContent);



        [WebInvoke(UriTemplate = "{a}", ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        [OperationContract]
        TestClass Test(string a, TestClass b);

        [WebInvoke(UriTemplate = "get", Method = "POST")]
        string GetTest();
    }
    [DataContract]
    public class TestClass
    {
        [DataMember(EmitDefaultValue = false)]
        public string a { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string b { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TestClass1 c1 { get; set; }
    }

    [DataContract]
    public class TestClass1
    {
        [DataMember(EmitDefaultValue = false)]
        public string aaa { get; set; }
    }
}
