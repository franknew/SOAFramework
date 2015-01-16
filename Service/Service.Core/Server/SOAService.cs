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

namespace SOAFramework.Service.Server
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class SOAService : IService
    {
        private static string _filePath = "";

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            ServerResponse response = new ServerResponse();
            Stopwatch watch = new Stopwatch();
            Stopwatch allWatch = new Stopwatch();
            allWatch.Start();
            string json = "";
            try
            {
                #region 准备工作
                string methodFullName = typeName + "." + functionName;
                ServiceModel service = ServicePoolManager.GetItem<ServiceModel>(methodFullName);
                MethodInfo method = null;
                if (service != null)
                {
                    method = service.MethodInfo;
                }
                //如果找不到方法重新加载配置的DLL
                else
                {
                    ServiceUtility.InitBusinessCache();
                    service = ServicePoolManager.GetItem<ServiceModel>(methodFullName);
                    if (service != null)
                    {
                        method = service.MethodInfo;
                    }
                }
                //如果再找不到方法，说明没有配置
                if (method == null)
                {
                    throw new Exception("未能找到接口：" + methodFullName + "！");
                }
                Dictionary<string, object> parsedArgs = new Dictionary<string, object>();
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        if (args.ContainsKey(p.Name))
                        {
                            parsedArgs[p.Name] = JsonHelper.Deserialize(args[p.Name], p.ParameterType);
                        }
                    }
                }
                #endregion

                #region 执行前置filter
                IFilter failedFilter = ServiceUtility.FilterExecuting(_filterList, typeName, functionName, method, parsedArgs);
                if (failedFilter != null)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                }
                #endregion

                #region 执行方法
                if (!response.IsError)
                {
                    try
                    {
                        watch.Start();
                        //执行方法
                        object result = ServiceUtility.ExecuteMethod(typeName, functionName, parsedArgs);
                        watch.Stop();
                        response.Data = result;
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    }
                    catch (Exception ex)
                    {
                        response.IsError = true;
                        response.ErrorMessage = ex.Message;
                        response.StackTrace = ex.StackTrace;
                    }
                }
                #endregion

                #region 执行后置filter
                failedFilter = ServiceUtility.FilterExecuted(_filterList, typeName, functionName, method, parsedArgs, watch.ElapsedMilliseconds, response);
                if (failedFilter != null && !response.IsError)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                response.StackTrace = ex.StackTrace;
            }

            #region 处理结果
            //序列化对象成json
            if (response.IsError)
            {
                json = JsonHelper.Serialize(response);
            }
            else
            {
                json = JsonHelper.Serialize(response.Data);
            }
            //压缩json
            string zippedJson = ZipHelper.Zip(json);
            allWatch.Stop();
            if (enableConsoleMonitor)
            {
                Console.WriteLine("{0}.{1} -- 耗时：{2}", typeName, functionName, allWatch.ElapsedMilliseconds);
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
            #endregion
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
