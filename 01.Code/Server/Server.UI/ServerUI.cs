using SOAFramework.Service.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Windows.Forms;
using SOAFramework.Library;
using SOAFramework.Service.Server;
using SOAFramework.Service.Core;
using System.ServiceModel;

namespace SOAFramework.Server.UI
{
    public partial class ServerUI : BaseUI
    {
        internal AppDomain domain;
        private BackgroundWorker worker = new BackgroundWorker();
        private ToolStripStatusLabel tssHostIp = new ToolStripStatusLabel("服务地址：暂无");
        private ToolStripStatusLabel tssCpu = new ToolStripStatusLabel();
        private ToolStripStatusLabel tssSep1 = new ToolStripStatusLabel("|");
        private ToolStripStatusLabel tssSep2 = new ToolStripStatusLabel("|");
        private ToolStripStatusLabel tssRam = new ToolStripStatusLabel();
        private Performance performance = new Performance();
        private ToolStripStatusLabel tssDispatcher = new ToolStripStatusLabel();
        private bool _isError = false;

        public ServerUI()
        {
            InitializeComponent();
            tbStart.Click += tbStart_Click;
            tbStop.Click += tbStop_Click;
            tbStop.Enabled = false;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            tssCpu.AutoSize = false;
            tssCpu.TextAlign = ContentAlignment.MiddleLeft;
            tssCpu.Width = 100;
            tssRam.AutoSize = false;
            tssRam.Width = 120;
            tssRam.TextAlign = ContentAlignment.MiddleLeft;
            ssBar.Items.Add(tssHostIp);
            ssBar.Items.Add(tssSep1);
            ssBar.Items.Add(tssCpu);
            ssBar.Items.Add(tssRam);
            ssBar.Items.Add(tssSep2);
            ssBar.Items.Add(tssDispatcher);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!e.Cancel)
            {
                host.Close();
                worker.Dispose();
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (host.State == CommunicationState.Opened)
            {
                tbStart.Enabled = false;
                tbStop.Enabled = true;
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已启动" }, CacheEnum.FormMonitor);
            }
            else
            {
                tbStart.Enabled = true;
                tbStop.Enabled = false;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            host = null;

            try
            {
                host = new ServiceHost(typeof(JsonHost));
                host.Open();
                started = true;
                _isError = false;
                var endpoint = host.Description.Endpoints.FirstOrDefault(t => t.Contract.Name == typeof(JsonHost).Name);
                if (endpoint != null)
                {
                    tssHostIp.Text = "服务地址：" + endpoint.Address.Uri.AbsoluteUri;
                }
                if (!host.Ping())
                {
                    MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器启动超时" }, CacheEnum.FormMonitor);
                }
            }
            catch (Exception ex)
            {
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = ex.Message }, CacheEnum.FormMonitor);
                _isError = true;
            }
        }

        private Task hostTask;
        private ServiceHost host;

        private void tbStart_Click(object sender, EventArgs e)
        {
            tbStart.Enabled = false;
            MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务正在启动中..." }, CacheEnum.FormMonitor);
            worker.RunWorkerAsync();
        }

        private void tbStop_Click(object sender, EventArgs e)
        {
            if (host != null && host.State == CommunicationState.Opened && !_isError)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            tssStatus.Text = "状态：" + GetServiceStatusDesc(host);
        }

        private string GetServiceStatusDesc(ServiceHost host)
        {
            string status = "";
            if (host == null)
            {
                status = "未启动";
                return status;
            }
            switch (host.State)
            {
                case CommunicationState.Closed:
                    status = "已关闭";
                    break;
                case CommunicationState.Closing:
                    status = "关闭中...";
                    break;
                case CommunicationState.Opened:
                    status = "已启动";
                    break;
                case CommunicationState.Opening:
                    status = "启动中...";
                    break;
                case CommunicationState.Created:
                    status = "已创建";
                    break;
                case CommunicationState.Faulted:
                    status = "发生错误";
                    break;
            }
            return status;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DisplayMachineStatus();
        }

        private void DisplayMachineStatus()
        {
            tssCpu.Text = "CPU使用率：" + performance.GetCurrentCpuUsage().ToString("N0") + "%";
            tssRam.Text = "可用内存：" + performance.GetCurrentRamUsage() + "MB";
        }

        private void ServerUI_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DispatcherServerUrl"]))
            {
                tssDispatcher.Text = "是否分发服务器：是";
            }
            else
            {
                tssDispatcher.Text = "分发服务器地址：" + ConfigurationManager.AppSettings["DispatcherServerUrl"];
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationName"]))
            {
                this.Text = ConfigurationManager.AppSettings["ApplicationName"];
            }
            DisplayMachineStatus();
            timer2.Start();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AutoStart"]))
            {
                tbStart_Click(sender, e);
            }
        }
    }
}
