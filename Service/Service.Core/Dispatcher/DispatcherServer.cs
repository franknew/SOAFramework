using SOAFramework.Library;
using SOAFramework.Service.Model;
using SOAFramework.Service.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOAFramework.Service.Core
{
    public class DispatcherServer : IDispatcher
    {
        Stream IDispatcher.Execute(string typeName, string functionName, Dictionary<string, string> args, List<IFilter> filterList,
            bool enableConsoleMonitor)
        {
            try
            {
                string url = ServiceUtility.GetMinCpuDispatcher();
                string executeUrl = string.Format("{0}/Execute/{1}/{2}", url.TrimEnd('/'), typeName, functionName);
                //分配到自己
                if (string.IsNullOrEmpty(url) || url == "Server")
                {
                    return ServiceUtility.Execute(typeName, functionName, args, filterList, enableConsoleMonitor);
                }
                else//分配到别的服务器
                {
                    List<PostArgItem> list = new List<PostArgItem>();
                    foreach (var key in args.Keys)
                    {
                        PostArgItem arg = new PostArgItem
                        {
                            Key = key,
                            Value = args[key],
                        };
                    }
                    byte[] data = Encoding.UTF8.GetBytes(JsonHelper.Serialize(list));
                    string result = HttpHelper.Post(executeUrl, data);
                    return new MemoryStream(Encoding.UTF8.GetBytes(result));
                }
            }
            catch (Exception ex)
            {
                ServerResponse response = new ServerResponse();
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                
                string json = JsonHelper.Serialize(response.Data);
                string zippedJson = ZipHelper.Zip(json);
                return new MemoryStream(Encoding.UTF8.GetBytes(zippedJson));
            }
        }

        void IDispatcher.StartRegisterTask(string dispatchServerUrl)
        {
            while (true)
            {
                ServiceUtility.RegisterDispatcher(dispatchServerUrl, ServiceUtility.GetCpuRate());

                Thread.Sleep(1000);
            }
        }
    }
}
