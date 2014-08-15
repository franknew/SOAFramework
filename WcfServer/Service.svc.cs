using SOAFramework.WcfServer;
//using SOAFramework.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SOAFramework.WcfServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service : IService
    {
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        public string Execute(string json)
        {
            string result = "";
            //前置层
            //解压缩
            //result = ZipHelper.UnZip(json);
            //解密
            //反序列化
            //执行方法
            //后置层
            //返回结果
            return result;
        }

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        public TestClass JsonTest(string input)
        {
            TestClass a = new TestClass();
            a.name = "aaaa";
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return a;
        }
        [WebInvoke(Method = "GET")]
        [OperationContract]
        public string Get()
        {
            return "i'm working";
        }
    }

    [DataContract()]
    public class TestClass
    {
        public string value { get; set; }
        public string name { get; set; }
    }
}
