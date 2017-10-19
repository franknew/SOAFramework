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
    [ServiceLayer(IsHiddenDiscovery = true)]
    public class DispatcherServer : IDispatcher
    {
        Stream IDispatcher.Execute(string typeName, string functionName, Dictionary<string, object> args, List<BaseFilter> filterList,
            bool enableConsoleMonitor)
        {
            try
            {
                string interfaceName = ServicePool.Instance.GetIntefaceName(typeName, functionName);
                bool callSuccess = false;
                Stream stream = null;
                while (!callSuccess)
                {
                    string url = ServicePool.Instance.GetMinCpuDispatcher();
                    //分配到自己
                    if (string.IsNullOrEmpty(url) || url == "Server")
                    {
                        stream = ServicePool.Instance.Execute(typeName, functionName, args);
                        callSuccess = true;
                    }
                    else//分配到别的服务器
                    {
                        #region 分发到其他服务器
                        IDispatcherExecuter exec = DispatcherExecuterFactory.CreateExecuter(DispatcherExecuterType.Http);
                        string result = null;
                        try
                        {
                            MonitorCache.GetInstance().PushMessage(
                                new CacheMessage { Message = string.Format("接口：{0}开始分发到服务：{1}！", interfaceName, url), MessageType = enumMessageType.Info, TimeStamp = DateTime.Now },
                                CacheEnum.LogMonitor);
                            result = exec.Execute(url, typeName, functionName, args);
                            MonitorCache.GetInstance().PushMessage(
                                new CacheMessage { Message = string.Format("接口：{0}已经分发到服务：{1}并执行成功！", interfaceName, url), MessageType = enumMessageType.Info, TimeStamp = DateTime.Now },
                                CacheEnum.LogMonitor);
                            stream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                            callSuccess = true;
                        }
                        catch (TimeoutException ex)
                        {
                            ServicePool.Instance.RemoveDispatcher(url);
                            MonitorCache.GetInstance().PushMessage(
                                new CacheMessage { Message = string.Format("接口：{0}已经分发到服务：{1}，连接服务器失败！", interfaceName, url), MessageType = enumMessageType.Info, TimeStamp = DateTime.Now },
                                CacheEnum.LogMonitor);
                        }
                        #endregion
                    }
                }
                return stream;
            }
            catch (Exception ex)
            {
                ServerResponse response = new ServerResponse();
                response.IsError = true;
                response.ErrorMessage = ex.Message;

                return response.ToStream();
            }
        }

        void IDispatcher.StartRegisterTask(string dispatchServerUrl)
        {
            while (true)
            {
                ServicePool.Instance.RegisterDispatcher(dispatchServerUrl, ServicePool.Instance.GetCpuRate());

                Thread.Sleep(1000);
            }
        }
    }
}
