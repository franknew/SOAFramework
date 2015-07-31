using SOAFramework.Service.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOAFramework.Service.Host
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“JsonHost”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 JsonHost.svc 或 JsonHost.svc.cs，然后开始调试。
    public class JsonHost : IJsonHost
    {
        private static readonly SOAService service = new SOAService();

        public System.IO.Stream Execute(string typeName, string functionName, Stream args)
        {
            string json = new StreamReader(args).ReadToEnd();
            return service.Execute(typeName, functionName, json);
        }

        public bool Ping()
        {
            return service.Ping();
        }

        public System.IO.Stream Download(string fileName)
        {
            return service.Download(fileName);
        }

        public System.IO.Stream Upload(string fileName, string fileContent)
        {
            return service.Upload(fileName, fileContent);
        }

        public void RegisterDispatcher(string usage, string url)
        {
            service.RegisterDispatcher(usage, url);
        }
    }
}
