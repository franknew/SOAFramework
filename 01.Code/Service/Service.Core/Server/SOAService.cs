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
using SOAFramework.Service.Core;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using SOAFramework.Service.Core.Model;
using System.Dynamic;
using System.Web;

namespace SOAFramework.Service.Server
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SOAService : IService
    {
        private static IDispatcher _dispatcher = null;

        private static string _filePath = "";

        private static bool enableConsoleMonitor = false;

        private static bool _isError = false;

        private static List<BaseFilter> _filterList = new List<BaseFilter>();
        static SOAService()
        {
            try
            {
                //把DLL中的所有方法加载到缓存中
                ServicePool.Instance.Init();
                if (string.IsNullOrEmpty(ServicePool.Instance.DispatchServerUrl))
                {
                    _dispatcher = new DispatcherServer();
                }
                else
                {
                    _dispatcher = new DispatcherClient();
                }

                if (ServicePool.Instance.EnableRegDispatcher)
                {
                    Task task = new Task(() =>
                    {
                        _dispatcher.StartRegisterTask(ServicePool.Instance.DispatchServerUrl);
                    });
                    task.Start();
                }
            }
            catch (Exception ex)
            {
                Exception exinner = ex; 
                StringBuilder stacktrace = new StringBuilder();
                StringBuilder message = new StringBuilder();
                while (exinner.InnerException != null)
                {
                    stacktrace.Append(exinner.StackTrace);
                    message.Append(exinner.Message);
                    exinner = exinner.InnerException;
                }
                StringBuilder log = new StringBuilder();
                log.AppendFormat("Message:{0} \r\n Stack Trace:{1}", message.ToString(), stacktrace.ToString());
                string logPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Logs\\";
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                File.AppendAllText(logPath + DateTime.Now.ToString("yyMMddHH") + ".log", log.ToString());
                _isError = true;
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
        public Stream Execute(string typeName, string functionName, string args)
        {
            Stream stream = null;
            if (_isError)
            {
                ServerResponse response = new ServerResponse();
                response.IsError = true;
                response.ErrorMessage = "系统初始化时报错！";
                stream = response.ToStream();
                return stream;
            }
            Dictionary<string, object> dic = null;
            try
            {
                dic = JsonHelper.Deserialize<Dictionary<string, object>>(args);
            }
            catch (Exception ex)
            {
                ServerResponse response = new ServerResponse();
                response.IsError = true;
                response.StackTrace = ex.StackTrace;
                response.ErrorMessage = "反序列化传入json字符串时出错！Json:" + args;
                stream = response.ToStream();
                return stream;
            }
            try
            {
                stream = _dispatcher.Execute(typeName, functionName, dic, _filterList, enableConsoleMonitor);
            }
            catch (Exception ex)
            {
                ServerResponse response = new ServerResponse();
                response.IsError = true;
                response.StackTrace = ex.StackTrace;
                response.ErrorMessage = ex.Message;
                stream = response.ToStream();
                return stream;
            }
            return stream;
        }

        [ServiceInvoker(IsHiddenDiscovery = true)]
        public void RegisterDispatcher(string usage, string url)
        {
            float rate = 0;
            float.TryParse(usage, out rate);
            string strUrl = url;
            ServicePool.Instance.RegisterDispatcher(strUrl, rate);
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

        public bool Ping()
        {
            return !_isError;
        }

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


        public string PostTest(string data)
        {
            return data;
        }
    }
}
