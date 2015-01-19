using SOAFramework.Service.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Windows.Forms;
using SOAFramework.Library;
using SOAFramework.Service.Server;
using SOAFramework.Service.Core;

namespace SOAFramework.Server.UI
{
    public partial class ServerUI : BaseUI
    {
        internal AppDomain domain;
        private BackgroundWorker worker = new BackgroundWorker();

        public ServerUI()
        {
            InitializeComponent();
            tbStart.Click += tbStart_Click;
            tbStop.Click += tbStop_Click;
            tbStop.Enabled = false;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (host.State == System.ServiceModel.CommunicationState.Opened)
            {
                tbStart.Enabled = false;
                tbStop.Enabled = true;
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已启动" }, CacheEnum.FormMonitor);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            host = new WebServiceHost(typeof(SOAService));
            try
            {
                host.Open();
                if (host.Ping())
                {
                }
                else
                {
                    MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器启动超时" }, CacheEnum.FormMonitor);
                }
            }
            catch (Exception ex)
            {
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = ex.Message }, CacheEnum.FormMonitor);
            }
        }

        private Task hostTask;
        private WebServiceHost host; 

        private void tbStart_Click(object sender, EventArgs e)
        {
            //if (domain == null)
            //{
            //    domain = AppDomain.CreateDomain("SOAServiceDomain");
            //}
            worker.RunWorkerAsync();
        }

        private void tbStop_Click(object sender, EventArgs e)
        {
            if (host != null && host.State == System.ServiceModel.CommunicationState.Opened)
            {
                host.Close(); 
                tbStart.Enabled = true;
                tbStop.Enabled = false;
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已停止" }, CacheEnum.FormMonitor);
            }
            //if (domain != null)
            //{
            //    AppDomain.Unload(domain);
            //}
        }
    }
}
