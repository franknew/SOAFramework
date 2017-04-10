using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SOAFramework.Service.Core;
using SOAFramework.Service.Server;
using System.Dynamic;
using System.ServiceModel.Activation;
using SOAFramework.Service.Core.Model;

namespace SOAFramework.Service.Host
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SOAServiceHost : IService
    {
        private static readonly SOAService service = new SOAService();

        Stream IService.Download(string fileName)
        {
            return service.Download(fileName);
        }

        Stream IService.Execute(string typeName, string functionName, string args)
        {
            return service.Execute(typeName, functionName, args);
        }

        string IService.GetTest()
        {
            throw new NotImplementedException();
        }

        bool IService.Ping()
        {
            return service.Ping();
        }

        void IService.RegisterDispatcher(string usage, string url)
        {
            throw new NotImplementedException();
        }

        Stream IService.Upload(string fileName, string fileContent)
        {
            return service.Upload(fileName, fileContent);
        }


        public string PostTest(string data)
        {
            return service.PostTest(data);
        }


        public Stream PostStream(Stream args)
        {
            return args;
        }
    }

   
}
