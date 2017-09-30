

using SOAFramework.Library;
using SOAFramework.Library.Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.alibaba.rocketmq.client.consumer;
using com.alibaba.rocketmq.client.producer;
using System.Reflection;
using com.alibaba.rocketmq.common.consumer;
using com.alibaba.rocketmq.client.consumer.listener;
using com.alibaba.rocketmq.remoting;
using Chainway.SSO;
using SOAFramework.Library.Cache;
using System.Runtime.Caching;

namespace WinformTest
{
    [TestAttibute]
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            //config.Servers.Add(new IPEndPoint(IPAddress.Loopback, 11211));
            //config.Protocol = MemcachedProtocol.Text;
            //config.Authentication.Type = typeof(PlainTextAuthenticator);
            //config.Authentication.Parameters["zone"] = "memcached";
            //config.Authentication.Parameters["userName"] = "Administrator";
            //config.Authentication.Parameters["password"] = "frank";

            //var section = (MemcachedClientSection)MemcachedClient.GetSection("enyim.com/memcached");

            //config = new MemcachedClientConfiguration();
            //foreach (EndPointElement s in section.Servers)
            //{
            //    config.Servers.Add(s.EndPoint);
            //}
            //config.Protocol = section.Protocol;
            ////config1.Protocol = MemcachedProtocol.Text;
            //var mc = new MemcachedClient(section);
            ////var mc = new MemcachedClient(config);

            //for (var i = 0; i < 100; i++)
            //    mc.Store(StoreMode.Add, "memcached", "World");

        }

        private void btnAddToFirewall_Click(object sender, EventArgs e)
        {
            FirewallManager manager = new FirewallManager();
            manager.AddPortToWhiteList(txbName.Text, Convert.ToInt32(txbPort.Text), ProtocolEnum.TCP);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            FirewallManager manager = new FirewallManager();
            manager.DeletePortFromWhiteList(Convert.ToInt32(txbPort.Text), ProtocolEnum.TCP);
        }

        private void btnInvoke_Click(object sender, EventArgs e)
        {
            string data = ConfigurationManager.AppSettings["data"];
            var urls = ConfigurationManager.AppSettings["url"];
            StringBuilder builder = new StringBuilder();
            var datas = data.Split('$');
            var urlarr = urls.Split(';');
            var urlList = urlarr.ToList();
            urlList.RemoveAll(t => string.IsNullOrEmpty(t.Trim()));
            foreach (var url in urlarr)
            {
                var dataList = datas.ToList();
                dataList.RemoveAll(t => string.IsNullOrEmpty(t.Trim()));
                Stopwatch watch = new Stopwatch();
                foreach (var d in datas)
                {
                    Task.Factory.StartNew(() =>
                    {

                    });
                    if (string.IsNullOrEmpty(d)) continue;
                    watch.Start();
                    chainway way = new chainway();
                    way.go("aaa", "111", d, url);
                    watch.Stop();
                }
                builder.AppendFormat("调用接口次数：{0} 总时间：{1} 平均时间：{2}  url:{3}", dataList.Count, watch.ElapsedMilliseconds, watch.ElapsedMilliseconds / dataList.Count, url).AppendLine();
            }
            lblContent.Text = builder.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //WindowsServiceController c = new WindowsServiceController("119.147.144.86", "administrator", "^%$#@!");
            WindowsServiceController c = new WindowsServiceController("localhost", "administrator", "frank");
            c.Connect();
            var list = c.GetServices("");
        }

        private void btnmq_Click(object sender, EventArgs e)
        {

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(textBox1.Text);
            var files = d.GetFiles("*.jar", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                f.CopyTo(@"E:\jars\" + f.Name, true);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(txbJarPath.Text);
            var files = d.GetFiles("*.jar", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                string outfilePath = @"E:\jartodll\dll\" + f.Name.TrimEnd('.', 'j', 'a', 'r') + ".dll";
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo
                {
                    Arguments = " -out:" + outfilePath + " " + f.FullName,
                    FileName = @"D:\software\ikvm-7.2.4630.5\bin\ikvmc.exe",
                };
                p.Start();
                p.WaitForExit();
            }


        }

        private void btnConsume_Click(object sender, EventArgs e)
        {
        }

        private void btnJVM_Click(object sender, EventArgs e)
        {
            //var connector = new JavaConnector("");
            //connector.InitializeJVM();
            //Java j = new Java();
            var services = ResolverHelper.ResolveDomain(AppDomain.CurrentDomain, AttributeTargets.Class, (t =>
            {
                return t.GetCustomAttribute<TestAttibute>(true) != null;
            }));
            services = ResolverHelper.ResolveWebApi(services);
        }

        /// <summary>
        /// 测试登录
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="entities">nonono</param>
        /// <returns>返回值</returns>
        public LoginEntity login(string name, List<LoginEntity> entities)
        {
            return null;
        }

        private void btnRedisSet_Click(object sender, EventArgs e)
        {
            ICache cache = CacheFactory.Create(CacheType.Redis);
            
            //cache.AddItem("test2", item, -1);
            var item1 = cache.GetItem<CacheItem>("test2");
        }

        private void btnDotNetty_Click(object sender, EventArgs e)
        {

        }
    }
}
