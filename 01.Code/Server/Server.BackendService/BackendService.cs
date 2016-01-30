using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SOAFramework.Library;
using SOAFramework.Service.Server;
using System.Configuration;

namespace SOAFramework.Server
{
    public partial class BackendService : ServiceBase
    {
        private BackgroundWorker _worker = new BackgroundWorker();

        private ServiceHost host;
        private bool _isError = false;
        private static string _logPath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        private Thread _logthread = new Thread(new ThreadStart(Loging));

        public BackendService()
        {
            InitializeComponent();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogPath"])) _logPath = ConfigurationManager.AppSettings["LogPath"];
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已启动" }, CacheEnum.LogMonitor);
        }

        private static void Loging()
        {
            SimpleLogger _logger = new SimpleLogger(_logPath);
            while (1 == 1)
            {
                //处理日志信息
                List<CacheMessage> list = MonitorCache.GetInstance().PopMessages(CacheEnum.LogMonitor);
                while (list.Count > 0)
                {
                    CacheMessage message = list[0];
                    string text = string.Format("{0} -- Message:{1} -- Stack Trace:{2}", message.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), message.Message, message.StackTrace);
                    _logger.Write(text, false);
                    list.Remove(message);
                }
                Thread.Sleep(100);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            host = null;

            try
            {
                host = new ServiceHost(typeof(JsonHost));
                host.Open();
                _isError = false;
                var endpoint = host.Description.Endpoints.FirstOrDefault(t => t.Contract.Name == typeof(JsonHost).Name);
            }
            catch (Exception ex)
            {
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = ex.Message }, CacheEnum.LogMonitor);
                _isError = true;
            }
        }

        protected override void OnStart(string[] args)
        {
            MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务正在启动中..." }, CacheEnum.LogMonitor);
            _worker.RunWorkerAsync();
            _logthread.Start();
            
        }

        protected override void OnStop()
        {
            if (host != null && host.State == CommunicationState.Opened && !_isError)
            {
                host.Close();
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已停止" }, CacheEnum.LogMonitor);
            }
        }
    }
}
