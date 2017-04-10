using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using SOAFramework.Service.Server;
using SOAFramework.Library;

namespace SOAFramework.Server.WindowsService
{
    public partial class WindowsServiceServer : ServiceBase
    {
        //private BackgroundWorker _worker = new BackgroundWorker();

        //private ServiceHost host;
        //private bool _isError = false;
        //private Timer timer = new Timer(Timer_Callback, null, 0, 100);
        //private static TimerCallback Timer_Callback { get; set; }
        //private SimpleLogger _logger = new SimpleLogger("Logs");

        public WindowsServiceServer()
        {
            InitializeComponent();
            //_worker.DoWork += worker_DoWork;
            //_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //Timer_Callback = new TimerCallback(Timer_Run);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已启动" }, CacheEnum.LogMonitor);
        }

        private  void Timer_Run(object state)
        {
            //处理日志信息
            List<CacheMessage> list = MonitorCache.GetInstance().PopMessages(CacheEnum.LogMonitor);
            while (list.Count > 0)
            {
                CacheMessage message = list[0];
                string text = string.Format("{0} -- Message:{1} -- Stack Trace:{2}", message.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), message.Message, message.StackTrace);
                //_logger.Write(text, false);
                list.Remove(message);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //    host = null;

            //    try
            //    {
            //        host = new ServiceHost(typeof(JsonHost));
            //        host.Open();
            //        _isError = false;
            //        var endpoint = host.Description.Endpoints.FirstOrDefault(t => t.Contract.Name == typeof(JsonHost).Name);
            //    }
            //    catch (Exception ex)
            //    {
            //        MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = ex.Message }, CacheEnum.LogMonitor);
            //        _isError = true;
            //    }
        }

        protected override void OnStart(string[] args)
        {
            //MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务正在启动中..." }, CacheEnum.LogMonitor);
            //_logger.Write("started");
            //_worker.RunWorkerAsync();
            //Thread.Sleep(10000000);
        }

        protected override void OnStop()
        {
            //if (host != null && host.State == CommunicationState.Opened && !_isError)
            //{
            //    host.Close();
            //    MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已停止" }, CacheEnum.LogMonitor);
            //}
        }
    }
}
