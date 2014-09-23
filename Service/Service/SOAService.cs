using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Configuration;
using System.Collections;
using SOAFramework.Service.Interface;
using SOAFramework.Service.Model;
using System.IO;
using System.ServiceModel.Web;
using SOAFramework.Service.Core;
using SOAFramework.Library.Zip;

namespace SOAFramework.Service.Server
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class SOAService : IService
    {
        private static string _filePath = "";

        private static List<IFilter> _filterList = new List<IFilter>();
        static SOAService()
        {
            try
            {
                //把DLL中的所有方法加载到缓存中
                ServiceUtility.InitBusinessCache();
                _filterList = ServiceUtility.InitFilterList();
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
        public Stream Execute(string typeName, string functionName, Dictionary<string, object> args)
        {
            ServerResponse response = new ServerResponse();
            string json = "";
            try
            {
                string methodFullName = typeName + "." + functionName;
                MethodInfo method = ServicePoolManager.GetItem<MethodInfo>(methodFullName);
                bool valid = true;
                IFilter failedFilter = ServiceUtility.FilterExecuting(_filterList, typeName, functionName, method, args);
                if (failedFilter != null)
                {
                    valid = false;
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                    response.StackTrace = Environment.StackTrace;
                }
                if (valid)
                {
                    //执行方法
                    object result = ServiceUtility.ExecuteMethod(typeName, functionName, args);
                    response.Data = result;
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                }
                failedFilter = ServiceUtility.FilterExecuted(_filterList, typeName, functionName, method, args);
                if (failedFilter != null)
                {
                    response.IsError = true;
                    response.ErrorMessage = failedFilter.Message;
                    response.StackTrace = Environment.StackTrace;
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                response.StackTrace = ex.StackTrace;
            }
            if (response.IsError)
            {
                json = JsonHelper.Serialize(response);
            }
            else
            {
                json = JsonHelper.Serialize(response.Data);
            }
            string zippedJson = ZipHelper.Zip(json);
            return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
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
                _filePath = Directory.GetCurrentDirectory();
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
                    _filePath = Directory.GetCurrentDirectory();
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
        public TestClass Test(string a, TestClass b)
        {
            TestClass c = new TestClass();
            c.a = a;
            c.c1 = new TestClass1();
            c.c1.aaa = "hello";
            string result = JsonHelper.Serialize(b);
            result = WebUtility.HtmlEncode(result);
            //return result;
            return c;
        }

        public string GetTest()
        {
            Console.WriteLine("get test invoked");
            return "test successful";
        }
        #endregion
    }
}
