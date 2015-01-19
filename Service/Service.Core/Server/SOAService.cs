using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;
using System.Configuration;
using System.Collections;
using SOAFramework.Service.Interface;
using SOAFramework.Service.Core;
using System.IO;
using System.ServiceModel.Web;
using SOAFramework.Service.Core;
using SOAFramework.Library;
using SOAFramework.Service.Model;
using System.Threading.Tasks;

namespace SOAFramework.Service.Server
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SOAService : IService
    {
        private static IDispatcher _dispatcher = null;

        private static string _filePath = "";

        private static string _dispatcherServerUrl = "";

        private static bool enableConsoleMonitor = false;

        private static List<IFilter> _filterList = new List<IFilter>();
        static SOAService()
        {
            try
            {
                //把DLL中的所有方法加载到缓存中
                ServiceUtility.InitBusinessCache();
                _filterList = ServiceUtility.InitFilterList();

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableConsoleMonitor"]))
                {
                    if (ConfigurationManager.AppSettings["EnableConsoleMonitor"] == "1")
                    {
                        enableConsoleMonitor = true;
                    }
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DispatcherServerUrl"]))
                {
                    _dispatcherServerUrl = ConfigurationManager.AppSettings["DispatcherServerUrl"];
                }
                if (string.IsNullOrEmpty(_dispatcherServerUrl))
                {
                    _dispatcher = new DispatcherServer();
                }
                else
                {
                    _dispatcher = new DispatcherClient();
                }

                Task task = new Task(() =>
                {
                    _dispatcher.StartRegisterTask(_dispatcherServerUrl);
                });
                task.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SOAService()
        {
        }

        /// <summary>
        /// execute method 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns>return stream for pure json</returns>
        [ServiceInvoker(IsHiddenDiscovery = true)]
        public Stream Execute(string typeName, string functionName, Dictionary<string, string> args)
        {
            return _dispatcher.Execute(typeName, functionName, args, _filterList, enableConsoleMonitor);
        }

        [ServiceInvoker(IsHiddenDiscovery = true)]
        public void RegisterDispatcher(string usage, string url)
        {
            float rate = 0;
            float.TryParse(usage, out rate);
            string strUrl = url;
            ServiceUtility.RegisterDispatcher(strUrl, rate);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream Download(string fileName)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _filePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (!_filePath.EndsWith("\\"))
            {
                _filePath += "\\";
            }
            string fullFileName = _filePath + fileName;
            FileInfo file = new FileInfo(fullFileName);
            if (!file.Exists)
            {
                throw new System.IO.FileNotFoundException("文件:" + fileName + "未找到！");
            }
            string fileString = file.FileToString();
            //string zipped = ZipHelper.Zip(fileString);
            return new MemoryStream(Encoding.UTF8.GetBytes(fileString));
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public Stream Upload(string fileName, string fileContent)
        {
            ServerResponse response = new ServerResponse();
            string json = "";
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _filePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                if (!_filePath.EndsWith("\\"))
                {
                    _filePath += "\\";
                }
                string fullFileName = _filePath + fileName;
                fileContent.ToFile(fullFileName);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                response.StackTrace = ex.StackTrace;
            }
            json = JsonHelper.Serialize(response);
            string zippedJson = ZipHelper.Zip(json);
            return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
        }

        public void Ping()
        { }

        #region test
        [ServiceInvoker(Module = "Test")]
        public TestClass Test(string a, TestClass b)
        {
            TestClass c = new TestClass();
            c.a = a;
            c.c1 = new TestClass();
            c.c1.aaa = "hello";
            string result = JsonHelper.Serialize(b);
            result = WebUtility.HtmlEncode(result);
            //return result;
            return c;
        }

        [ServiceInvoker(Module = "Test")]
        public string GetTest()
        {
            Console.WriteLine("get test invoked");
            return "test successful";
        }

        public class TestClass
        {
            public string a { get; set; }

            public string aaa { get; set; }

            public TestClass c1 { get; set; }
        }
        #endregion
    }
}
