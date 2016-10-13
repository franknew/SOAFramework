using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AustinHarris.JsonRpc;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class NodeServer
    {
        private string url;
        const string sessionName = "__nodeMethods";
        private HttpServer httpServer;
        public ServerStatus Status
        {
            get { return httpServer.Status; }
            set { httpServer.Status = value; }
        }

        public string Url
        {
            get
            {
                return url;
            }
        }

        public string CommonDllPath { get; set; }

        public NodeServer(string url, string commonDllPath = null)
        {
            this.url = url;
            CommonDllPath = commonDllPath;
            httpServer = new HttpServer(new string[] { this.url });
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public NodeServer(string commonDllPath = null)
        {
            CommonDllPath = commonDllPath;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public NodeServer()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private string HttpServer_Executing(object sender, HttpExcutingEventArgs e)
        {
            var type = ContentTypeConvert.Convert(e.Request.ContentType);
            var serializor = SerializeFactory.Create(type);
            string rawUrl = e.Request.RawUrl.TrimEnd('/');
            int start = rawUrl.LastIndexOf("/");
            int end = rawUrl.LastIndexOf("?");
            if (end == -1) end = rawUrl.Length;
            string action = rawUrl.Substring(start + 1, end - start - 1);
            string post;
            using (StreamReader reader = new StreamReader(e.Request.InputStream, Encoding.UTF8))
            {
                post = reader.ReadToEnd();
            }
            Dictionary<string, object> args = serializor.Deserialize<Dictionary<string, object>>(post);
            if (args == null) args = new Dictionary<string, object>();
            foreach (var key in e.Request.QueryString.AllKeys)
            {
                if (!args.ContainsKey(key)) args[key] = e.Request.QueryString[key];
            }
            JsonResponse response = null;
            response = JsonRpcProcessor.ProcessReturnResponse(sessionName, new JsonRequest
            {
                Method = action,
                Params = args,
            }).FirstOrDefault();
            if (response == null) return "";
            else return serializor.Serialize(response);
        }

        public void Start(string url = null)
        {
            ServiceBinder.BindService(sessionName);
            if (!string.IsNullOrEmpty(url)) this.url = url;
            if (httpServer == null) httpServer = new HttpServer(new string[] { this.url });
            httpServer.Executing += HttpServer_Executing;
            httpServer.Start();
        }

        public void Stop()
        {
            httpServer.Stop();
        }

        public void Close()
        {
            httpServer.Close();
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            result = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.Equals(args.RequestingAssembly));
            if (result != null) return result;
            AssemblyName name = new AssemblyName(args.Name);
            string dllName = string.Format(@"{0}\{1}.dll", CommonDllPath.TrimEnd('\\'), name.Name);
            result = AssemblyHelper.LoadCopy(dllName);
            return result;
        }
    }
}
