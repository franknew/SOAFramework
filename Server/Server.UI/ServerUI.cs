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
using SOAFramework.Service.Server;
using SOAFramework.Library;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace SOAFramework.Server.UI
{
    public partial class ServerUI : BaseUI
    {

        public ServerUI()
        {
            InitializeComponent();
            tbStart.Click += tbStart_Click;
            tbStop.Click += tbStop_Click;
            tbStop.Enabled = false;
        }
        private string moduleDir = "Modules";
        private string dllCacheDir = "DllCache";

        public AppDomain ServiceDomain { get; set; }

        private Task hostTask;
        private WebServiceHost host; 

        private void tbStart_Click(object sender, EventArgs e)
        {
            if (ServiceDomain == null)
            {
                ServiceDomain = AppDomain.CreateDomain("SOAServiceDomain");
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuleDir"]))
                {
                    moduleDir = ConfigurationManager.AppSettings["ModuleDir"];
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DllCacheDir"]))
                {
                    dllCacheDir = ConfigurationManager.AppSettings["DllCacheDir"];
                }
                ServiceDomain.SetupInformation.ShadowCopyFiles = "true";
                string cacheDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + dllCacheDir;
                ServiceDomain.SetupInformation.CachePath = cacheDir;
                ServiceDomain.SetShadowCopyFiles();
                ServiceDomain.SetCachePath(cacheDir);
            }
            host = new WebServiceHost(typeof(SOAService));
            if (host != null)
            {
                hostTask = new Task(() =>
                {
                    host.Open();
                });
                hostTask.Start();
                tbStart.Enabled = false;
                tbStop.Enabled = true;
                MonitorCache.GetInstance().PushMessage(new CacheMessage { Message = "服务器已启动" }, CacheEnum.FormMonitor);
            }
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
            if (ServiceDomain != null)
            {
                AppDomain.Unload(ServiceDomain);
                ServiceDomain = null;
            }
        }

        
    }
}
