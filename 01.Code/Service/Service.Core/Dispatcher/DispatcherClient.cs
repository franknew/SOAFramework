using SOAFramework.Library;
using SOAFramework.Service.Core.Model;
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
    public class DispatcherClient : IDispatcher
    {
        Stream IDispatcher.Execute(string typeName, string functionName, Dictionary<string, string> args, List<BaseFilter> filterList,
               bool enableConsoleMonitor)
        {
            return ServiceUtility.Execute(typeName, functionName, args, filterList, enableConsoleMonitor);
        }

        void IDispatcher.StartRegisterTask(string dispatchServerUrl)
        {
            while (true)
            {
                try
                {
                    string currentEndPoint = ServiceUtility.GetCurrentEndPoint();
                    if (!string.IsNullOrEmpty(currentEndPoint))
                    {
                        
                        float cpuUsage = ServiceUtility.GetCpuRate();
                        string url = dispatchServerUrl.TrimEnd('/') + "/" + cpuUsage.ToString("N2");
                        byte[] byteArgs = Encoding.UTF8.GetBytes(JsonHelper.Serialize(currentEndPoint));
                        HttpHelper.Post(url, byteArgs);
                    }
                }
                catch
                {

                }
                Thread.Sleep(1000);
            }
        }
    }
}
