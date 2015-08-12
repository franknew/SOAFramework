using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SOAFramework.Service.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class JsonHost : IJsonHost
    {
        private static readonly SOAService service = new SOAService();

        public System.IO.Stream Execute(string typeName, string functionName, Stream args)
        {
            string json = new StreamReader(args).ReadToEnd();
            return service.Execute(typeName, functionName, json);
            //return args;
        }

        public bool Ping()
        {
            return service.Ping();
            //return true;
        }

        public System.IO.Stream Download(string fileName)
        {
            return service.Download(fileName);
            //return null;
        }

        public System.IO.Stream Upload(string fileName, string fileContent)
        {
            return service.Upload(fileName, fileContent);
            //return null;
        }

        public void RegisterDispatcher(string usage, string url)
        {
            service.RegisterDispatcher(usage, url);
        }
    }
}
