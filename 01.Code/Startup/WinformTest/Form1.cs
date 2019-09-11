

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
using System.Reflection;
using Chainway.SSO;
using SOAFramework.Library.Cache;
using System.Runtime.Caching;
using IBatisNet.DataMapper;
using SOAFramework.Library.DAL;
using System.Dynamic;
using SOAFramework.Library.Lib;
using SOAFramework.Service.SDK.Core;
using SOAFramework.Library.DAL.Generic;
//using JWT.Algorithms;
//using JWT.Serializers;
//using JWT;

namespace WinformTest
{
    [TestAttibute]
    public partial class Form1 : Form, IFormAction
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
            txbData.Text = builder.ToString();
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
            //var services = ResolverHelper.ResolveDomain(AppDomain.CurrentDomain, AttributeTargets.Class, (t =>
            //{
            //    return t.GetCustomAttribute<TestAttibute>(true) != null;
            //}));
            //services = ResolverHelper.ResolveWebApi(services);
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

        private void btnDalQuery_Click(object sender, EventArgs e)
        {
            //List<Task> tasks = new List<Task>();
            //SimpleLogger logger = new SimpleLogger();
            //Stopwatch stop = new Stopwatch();
            //stop.Start();
            //for (int i = 0; i < 100000; i++)
            //{
            //    var task = Task.Factory.StartNew(() =>
            //    {
            //        try
            //        {
            //            var mapper = Mapper.Instance();
            //            UserDao dao = new UserDao();
            //            var list = dao.Query(new UserQueryForm { PageSize = 10, CurrentIndex = 1 });
            //            var json = JsonHelper.Serialize(list);
            //            logger.Write(json);
            //        }
            //        catch (Exception ex)
            //        {
            //            logger.WriteException(ex);
            //        }
            //    });
            //    tasks.Add(task);
            //}
            //Task.WaitAll(tasks.ToArray());
            //stop.Stop();
            //MessageBox.Show(stop.ElapsedMilliseconds.ToString());
        }

        private void btnFTS_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = File.ReadAllBytes(dialog.FileName);
                txbData.Text = Convert.ToBase64String(file);
            }
        }

        private void btnSTF_Click(object sender, EventArgs e)
        {
            var data = txbData.Text;
            var bytes = Convert.FromBase64String(data);
            File.WriteAllBytes("d:\\test.jpg", bytes);
        }

        private void btnSDKTesting_Click(object sender, EventArgs e)
        {

            //ServiceDiscoveryController c = new ServiceDiscoveryController();
            //c.Api("GET-api-ServiceDiscovery-Api");
        }

        private void btnChangeType_Click(object sender, EventArgs e)
        {
            var b = "0".ChangeTypeTo<byte[]>();
        }

        private void btnsql_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            helper.ExecNoneQueryWithSQL(txbData.Text);
            MessageBox.Show("ok");
        }

        private void btnDynamic_Click(object sender, EventArgs e)
        {
            //Dictionary<string, TypeModel> dic = new Dictionary<string, TypeModel>();
            ////MessageBox.Show(dic.GetType().FullName);
            //string mainType = dic.GetType().FullName.Split('`')[0];
            //string genericTypes = dic.GetType().FullName.Split('`')[1];
            //genericTypes = genericTypes.Trim('[', ']');
            //var types = genericTypes.GetVairable('[', ']');
            //PaginationResult<BaseEntity> result = new PaginationResult<BaseEntity>();
            ////MessageBox.Show(result.GetType().FullName);
            ////MessageBox.Show(typeof(PaginationResult<>).FullName);
            //var ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.FullName.StartsWith("SOAFramework.Library.DAL"));
        }

        void IFormAction.OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void btnZipPackage_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            files.Add(@"e:\test.txt");
            files.Add(@"e:\abc.txt");
            files.Add(@"e:\CodeSmithTemplate.rar");
            ZipHelper.PackageFiles(@"e:\test.zip", files);
        }

        private void btnJWT_Click(object sender, EventArgs e)
        {

            //string header = txbHeader.Text;
            //string payload = txbPayload.Text;
            //string secret = txbSecret.Text;

            //IDictionary<string, object> payloadDic = JsonHelper.Deserialize<IDictionary<string, object>>(payload);
            //var token = JWTHelper.Encode(secret, payloadDic);
            //txbData.Text = token;
        }

        private void btnJWTDecode_Click(object sender, EventArgs e)
        {
            //string secret = txbSecret.Text;
            //string token = txbData.Text;
            //var json = JWTHelper.Decode(token, secret);
            //txbPayload.Text = json;
        }

        private void btnLogAsync_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            SimpleLogger logger = new SimpleLogger();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                logger.LogAsync("hello world, i=" + i.ToString());
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            MessageBox.Show("async over:" + stopwatch.ElapsedMilliseconds);
        }

        private void btnLogSync_Click(object sender, EventArgs e)
        {
            SimpleLogger logger = new SimpleLogger();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                logger.Log("hello world, i=" + i.ToString());
            }
            stopwatch.Stop();
            MessageBox.Show("sync over:" + stopwatch.ElapsedMilliseconds);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            //Performance performance = new Performance();
            //performance.GetCounterNameValueList("Proccess", ".");
        }

        Performance performance = new Performance();
        float cpu, mem, totalmem;

        private void btnSms_Click(object sender, EventArgs e)
        {
            //ISmsClient client = new AliSmsClient("LTAIGzdiAzBlQWpr", "36c0GTBkVIs232yWfxpJ3pg5tjETI6");
            //var response = client.Send("成为信息科技有限公司", "SMS_137425942", new { code = "334283" }, new string[] { "13823117810" });
            //MessageBox.Show(response.Message);
        }

        private void btnProxy_Click(object sender, EventArgs e)
        {
            //var t = InterfaceProxy.Create<ITest>(new HttpHandler());
            //var r = t.Go("aaa");
            
        }

        private void btnDalTest_Click(object sender, EventArgs e)
        {
            GenericOperation operation = new GenericOperation();
            DataTable table = operation.Query(new QueryEntity
            {
                TableName = "member",
                Columns = new List<string> { "id", "name" },
                Condition = new Condition
                {
                    Columns = new List<QueryColumn>
                    {
                        new QueryColumn("id", OperationTypeEnum.Equals, "1"),
                    },
                }
            });
        }

        string cpuString;

        private void Timer_Tick(object sender, EventArgs e)
        {
            cpu = performance.GetCurrentCpuUsage();
            //cpu = performance.CpuCounter.NextValue();
            mem = performance.GetAvailableRamSize();
            totalmem = performance.GetTotalMem();
            lblCpu.Text = cpu.ToString();
            lblTotalMem.Text = (totalmem / 1000000).ToString() + "MB";
            lblUsedMem.Text = (mem/1000000).ToString() + "MB";
        }
    }

    public class Test
    {
        public int? a { get; set; }
        public DateTime? b { get; set; }
    }

    public interface ITest
    {
        string Go(string a);
    }

}
