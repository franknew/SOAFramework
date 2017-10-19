using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using System.Collections;
using System.IO;

using SOAFramework.AOP;
using SOAFramework.WebServiceCore;
using SOAFramework.Library;
using SOAFramework.Controller;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using SOAFramework.Library.AOP;
using SOAFramework.Service.Server;
using System.ServiceModel;
using SOAFramework.Library.RazorEngine;
using System.Reflection;
using SOAFramework.Service.SDK.Core;
using SOAFramework.Service.Core;
using SOAFramework.Service.Core.Model;
using System.Linq.Expressions;
using System.Data.Linq;
using SOAFramework.Library.DAL;
//using MongoDB.Driver;
using System.Data;
using System.Threading;
//using RiskMgr.Form;
using SOAFramework.Library.WeiXin;
using System.Net;
using MicroService.Library;
using AustinHarris.JsonRpc;
using System.Xml.Serialization;

namespace Test
{
    public class Program
    {
        public Program()
        {
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
        }

        private static Assembly CurrentDomain_AssemblyResolve1(object sender, ResolveEventArgs args)
        {
            return args.RequestingAssembly;
        }

        public delegate void dl();
        static void Main(string[] args)
        {
            #region id generator
            IIDGenerator iDGenerator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
            List<string> idlist = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                idlist.Add(iDGenerator.Generate());
            }

            #endregion

            TestClass tc = new TestClass
            {
                tc = new TestClass
                {
                    a = "hello",
                    b = "word",
                    tc = new TestClass
                    {
                        a = "deep",
                    }
                },
                test = new List<TestClass>
                {
                    new TestClass
                    {
                        a = "<",
                    }
                },
            };
            #region xml testing
            TestRequest req = new TestRequest
            {
                c = new TestClass
                {
                    a = "a",
                    b = "b",
                    d = "d",
                    dt = DateTime.Now,
                }

            };
            ISerializable serial = new XmlSerializor();
            var xml = serial.Serialize(req);

            var obj = serial.Deserialize<TestRequest>(xml);

            Dictionary<string, object> arg = new Dictionary<string, object>(); 
            arg["a"] = "a";
            arg["b"] = 3;
            arg["c"] = req;
            xml = serial.Serialize(arg);
            var dicobj = serial.Deserialize<Dictionary<string, object>>(xml);
            #endregion
            //string jsonstring = "{\"Buyers\":[{\"ID\":\"395f7ce8de8340eda2dfd22098c81290\",\"Name\":\"爱的色放\",\"CardType\":\"1\",\"IdentityCode\":\"4444444444\",\"Phone\":\"123123123123\",\"Gender\":\"1\",\"Marrage\":\"1\",\"Address\":\"啊都是法师打发而且额外人\",\"OrignalName\":\"\",\"OrignalIdentityCode\":\"\",\"BankCode\":\"\",\"BankType\":\"1\",\"WorkUnit\":\"\",\"Quotient\":\"222\"},{\"ID\":\"\",\"Name\":\"阿萨法 \",\"CardType\":\"1\",\"IdentityCode\":\"986799283948723984\",\"Phone\":\"123123\",\"Gender\":\"2\",\"Marrage\":\"1\",\"Address\":\"三个地方集团研究研究\",\"OrignalName\":\"\",\"OrignalIdentityCode\":\"\",\"BankCode\":\"\",\"BankType\":\"\",\"WorkUnit\":\"\",\"Quotient\":\"333\"},{\"ID\":\"712feaff6c034244ab3f066268b9fe5a\",\"Name\":\"阿斯顿飞\",\"CardType\":\"1\",\"IdentityCode\":\"12312312312323\",\"Phone\":\"123123123\",\"Gender\":\"1\",\"Marrage\":\"1\",\"Address\":\"嘎达嗦嘎多个地方十多个地方各个\",\"OrignalName\":\"\",\"OrignalIdentityCode\":\"\",\"BankCode\":\"\",\"BankType\":\"1\",\"WorkUnit\":\"\",\"Quotient\":\"222\"}],\"Sellers\":[{\"ID\":\"55b71c225dc841a7b99ead4cecc601c5\",\"Name\":\"aeeboo\",\"CardType\":\"1\",\"IdentityCode\":\"234234235235\",\"Phone\":\"324234234234\",\"Gender\":\"1\",\"Marrage\":\"2\",\"Address\":\"的方式购房合同和投入和\",\"OrignalName\":\"\",\"OrignalIdentityCode\":\"\",\"BankCode\":\"\",\"BankType\":\"2\",\"WorkUnit\":\"\",\"Quotient\":\"111\"},{\"ID\":\"\",\"Name\":\"阿萨德飞44\",\"CardType\":\"1\",\"IdentityCode\":\"237856234\",\"Phone\":\"34234234\",\"Gender\":\"1\",\"Marrage\":\"1\",\"Address\":\"然后统一集团研究与\",\"OrignalName\":\"\",\"OrignalIdentityCode\":\"\",\"BankCode\":\"\",\"BankType\":\"\",\"WorkUnit\":\"\",\"Quotient\":\"123\"}],\"Assets\":[{\"ID\":\"\",\"Code\":\"44444444\",\"Usage\":\"1\",\"Position\":\"2\",\"Address\":\"景田西路八个道路\",\"Area\":\"123\",\"RegPrice\":\"44232\"},{\"ID\":\"\",\"Code\":\"1412412132\",\"Usage\":\"1\",\"Position\":\"1\",\"Address\":\"水电费个人个人高\",\"Area\":\"234324\",\"RegPrice\":\"123123\"}],\"Project\":{\"Source\":\"1\",\"AgentName\":\"213213\",\"CertificateData\":\"2015-08-05\",\"AgentContact\":\"\",\"Rebater\":\"\",\"RebateAccount\":\"\",\"OtherRebateInfo\":\"\",\"OrignalMortgageBank\":\"1\",\"OrignalMortgageBranch\":\"阿斯顿发顺丰\",\"OrignalFundCenter\":\"1\",\"OrignalFundBranch\":\"\",\"SupplyCardCopy\":\"\",\"OrignalCreditPI\":\"123123\",\"OrignalCreditCommerceMoney\":\"123\",\"OrignalCreditFundMoney\":\"123\",\"AssetRansomCustomerManager\":\"124142\",\"AssetRansomContactPhone\":\"24124\",\"NewCreditBank\":\"1\",\"NewCreditBranch\":\"2r323\",\"ShortTermAssetRansomBank\":\"1\",\"ShortTermAssetRansomBranch\":\"\",\"GuaranteeMoney\":\"123\",\"GuaranteeMonth\":\"1231\",\"BuyerCreditCommerceMoney\":\"213\",\"BuyerCreditFundMoney\":\"2\",\"LoanMoney\":\"123123\",\"DealMoney\":\"123123\",\"EarnestMoney\":\"123123\",\"SupervisionMoney\":\"123123\",\"SupervisionBank\":\"12123\",\"AssetRansomMoney\":\"122323\",\"CustomerPredepositMoney\":\"323232\",\"CreditReceiverName\":\"23123\",\"CreditReceiverBank\":\"2323\",\"CreditReceiverAccount\":\"2323\",\"TrusteeshipAccount\":\"\",\"AssetRansomPredictMoney\":\"2323\",\"AssetRansomer\":\"232323\",\"AssetRansomType\":\"1\",\"PredictDays\":\"2323\",\"ChargeType\":\"1\",\"CheckNumbersAndLimit\":\"123123\",\"Stagnationer\":\"\"},\"token\":\"0cbbd08b6b694428a30afe52098e5f7a\"}";
            //var json = JsonHelper.Deserialize<AddProjectServiceForm>(jsonstring);
            #region domain
            //AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve1;
            //AppDomainSetup info = new AppDomainSetup();
            //info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory + "Modules";
            //AppDomain domain = AppDomain.CreateDomain("mydomain", null, info);
            //Console.WriteLine(domain.BaseDirectory);
            //var allass = domain.GetAssemblies();
            //var ass = domain.Load(new AssemblyName("Controller"));
            //allass = domain.GetAssemblies();
            //var type = ass.GetType("SOAFramework.Library.HttpServer");
            //var instance = Activator.CreateInstance(type, null);
            //var start = type.GetMethod("Start");
            //start.Invoke(instance, new object[] { new string[] { "http://10.1.50.195:8094/c" } });
            //domain.AssemblyLoad += Domain_AssemblyLoad;
            #endregion

            #region http server
            Console.WriteLine("begin");
            NodeServer nodeserver = new NodeServer("http://10.1.50.195:8094/");
            nodeserver.Start();
            Console.ReadLine();
            //nodeserver.Close();

            HttpServer server = new HttpServer(new string[] { "http://10.1.50.195:8094/a" });
            server.Executing += new HttpExecutingHandler((a, b) =>
            {
                StreamReader reder = new StreamReader(b.Request.InputStream, System.Text.Encoding.UTF8);
                string post = reder.ReadToEnd();
                Console.WriteLine("key:" + post);
                return "";
            });
            server.Start();

            //HttpServer server2 = new HttpServer(new string[] { "http://10.1.50.195:8094/b" });
            //server2.Start();
            Console.ReadLine();

            string[] prefix = new string[] { "http://localhost:8088/", "http://localhost:8089/" };
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefix == null || prefix.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefix)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
                Thread.Sleep(1);
            }
            listener.Stop();
            Console.ReadLine();
            #endregion

            //var users = WeiXinApi.Department.Delete("广州研发中心");

            
            string[] listaar = new string[3];
            List<string> listaab = new List<string>();
            object stra = tc.GetValue("tc.tc.a");
            tc.SetValue("tc.tc.tc.b", "changed");
            object strb = tc.GetValue("test[0].a");
            tc.SetValue("tc.test[1].a", "aa");

            string jsonstring = "{dt: \"2015-08-09 11:22:33\"}";
            var dtd = JsonHelper.Deserialize<TestClass>(jsonstring);


            LogonRequest logonrequest = new LogonRequest();
            logonrequest.username = "admin";
            logonrequest.password = "admin";
            var res = SDKFactory.Client.Execute(logonrequest);

            IDBHelper helper = DBFactory.CreateDBHelper("Data Source=55a2335ca1f37.gz.cdb.myqcloud.com;Initial Catalog=RiskMgr;User Id=cdb_outerroot;Password=liuxiao7658490;Port=9582;", DBType.MySQL);
            DataTable dt = helper.GetTableWithSQL("SELECT * FROM Workflow");

            string testa = "b";
            int enumb = testa.ToEnumValue<TestEnum>();
            TestClass cc = new TestClass();
            //Update<TestClass>.Set(t => t.a, "");
            List<TestClass> lists = new List<TestClass>();
            lists.Add(new TestClass { a = "a" });
            lists.SetTest(t => t.a, "aaa");

            var c = (from a in lists
                     select a);
            var g = Select(new { a = "a", b = "b" });
            var d = new { a = "", b = "" };
            IEnumerable<TestClass> query = (from a in lists
                                            where a.a == "a"
                                            select a);

            var q = query.TestSelect(t => new { a = t.a });
            query.Select(t => t.test);
            //TestLinq<TestClass>(a => return new { a = "a" });
            DoAction<TestClass>(t => t.a = "b");
            string strData = "";
            byte[] data = null;
            string testresult = "";
            List<SOAFramework.Service.SDK.Core.PostArgItem> argslist = new List<SOAFramework.Service.SDK.Core.PostArgItem>();

            List<string> listT = new List<string>();
            Type[] ts = listT.GetType().GetGenericArguments();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Stopwatch watch = new Stopwatch();

            #region mapreduce
            Stopwatch sw = new Stopwatch();
            MemberInfo[] mem = sw.GetType().GetProperties();
            List<TestClass> listTestClass = new List<TestClass>();
            Random r = new Random();
            long allCount = 0;
            DataTable table = new DataTable();
            table.Columns.Add("a");
            table.Columns.Add("b", typeof(int));
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                allCount++;
                DataRow row = table.NewRow();
                row["a"] = i.ToString();
                row["b"] = r.Next(10);
                table.Rows.Add(row);
                //listTestClass.Add(new TestClass { a = i.ToString(), g = r.Next(10) });
                //listTestClass.Add(new TestClass { a = i.ToString(), g = i % 10 });
            }
            sw.Stop();
            Console.WriteLine("新增{1}个元素耗时：{0}", sw.ElapsedMilliseconds, allCount);
            allCount = 0;
            sw.Restart();
            var dic = table.MapReduce(t =>
            {
                return new KeyValueClass<int, string>(Convert.ToInt32(t.Row["b"]), t.Row["a"].ToString());
            }, (key, values) =>
            {
                return values.Count;
            });
            sw.Stop();
            Console.WriteLine("mapreduce耗时：{0}", sw.ElapsedMilliseconds);
            foreach (var key in dic.Keys)
            {
                allCount += dic[key];
                Console.WriteLine("key:{0}  value:{1}", key, dic[key]);
            }
            Console.WriteLine("所有元素总数：{0}", allCount);
            int x = listTestClass.Count(t => t.g == 0);

            //搜词测试
            List<string> wordMappingList = new List<string>();
            wordMappingList.Add("韩立");
            wordMappingList.Add("冰凤");
            wordMappingList.Add("南宫婉");
            wordMappingList.Add("真仙");
            wordMappingList.Add("真魔");
            wordMappingList.Add("马良");
            wordMappingList.Add("炼气");
            wordMappingList.Add("筑基");
            wordMappingList.Add("真丹");
            wordMappingList.Add("元婴");
            wordMappingList.Add("化神");
            wordMappingList.Add("化神");
            wordMappingList.Add("炼虚");
            wordMappingList.Add("合体");
            wordMappingList.Add("大乘");
            wordMappingList.Add("韩道友");
            //读取文章
            string words = File.ReadAllText("凡人修仙传.txt", System.Text.Encoding.GetEncoding("GBK"));
            FileInfo fileInfo = new FileInfo("凡人修仙传.txt");
            double size = fileInfo.Length;
            double sizeMB = (size / (1024 * 1024));
            sw.Restart();
            var dicWords = words.ToList().MapReduce(t =>
            {
                foreach (var w in wordMappingList)
                {
                    bool valid = true;
                    if (w[0].Equals(t.Data))
                    {
                        for (int i = 1; i < w.Length; i++)
                        {
                            if (!w[i].Equals(t.List[t.Index + i]))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid)
                        {
                            return new KeyValueClass<string, int>(w, 1);
                        }
                    }
                }
                return KeyValueClass<string, int>.Empty();
            }, (key, values) =>
            {
                return values.Count;
            });
            sw.Stop();
            Console.WriteLine("词频统计 - 文件大小：{1}MB  测试耗时：{0}", sw.ElapsedMilliseconds, sizeMB.ToString("N2"));
            foreach (var key in wordMappingList)
            {
                if (dicWords.ContainsKey(key))
                {
                    Console.WriteLine("{0}出现次数：{1}", key, dicWords[key]);
                }
            }

            Console.ReadLine();
            #endregion

            #region codesmith testing

            strData = JsonHelper.Serialize(argslist);
            data = System.Text.Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/Execute/SOAFramework.Service.Server.DefaultService/DiscoverService", data);
            testresult = ZipHelper.UnZip(testresult);
            List<ServiceInfo> serviceList = JsonHelper.Deserialize<List<ServiceInfo>>(testresult);
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\Templates\SDKRequest.cst";
            Dictionary<string, object> argsCodeSmith = new Dictionary<string, object>();
            argsCodeSmith["RequestNameSpace"] = "a.b.c";
            argsCodeSmith["ServiceInfo"] = serviceList[0];
            string render = CodeSmithHelper.GenerateString(path, argsCodeSmith);

            string fileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\SOAFramework.Library.CodeSmithConsole.exe ";
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = " " + path.Replace(@"\\", @"\" + " ") + " " + JsonHelper.Serialize(argsCodeSmith).Replace("\"", "\\\"") + " ";
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.WaitForExit();
            return;
            #endregion

            #region razor
            //string strr = File.ReadAllText("Temp.txt");
            //Dictionary<string, object> dicargs = new Dictionary<string, object>();
            //dicargs["a"] = "22222";
            //string r = Razor.Parse(strr, dicargs);
            #endregion

            #region json tester
            //List<TestClass> list = new List<TestClass>();
            //for (int i = 0; i < 10; i++)
            //{
            //    TestClass c = new TestClass
            //    {
            //        a = "a" + i.ToString(),
            //        b = "b" + i.ToString(),
            //        dic = new Dictionary<string, string>(),
            //    };
            //    c.dic["dic1"] = "dic1" + i.ToString();
            //    c.dic["dic2"] = "dic2" + i.ToString();
            //    c.dic["dic3"] = "dic3" + i.ToString();
            //    c.test = new List<TestClass>();
            //    c.test.Add(new TestClass { a = "aa" + i.ToString(), b = "bb" + i.ToString() });
            //    c.test.Add(new TestClass { a = "cc" + i.ToString(), b = "dd" + i.ToString() });
            //    list.Add(c);
            //}
            //watch.Start();
            //string strjson = JsonHelper.Serialize(list, false);
            //watch.Stop();
            //Console.WriteLine("序列化：{0}", watch.ElapsedMilliseconds);
            //watch.Reset();
            //watch.Start();
            //List<TestClass> list1 = JsonHelper.Deserialize<List<TestClass>>(strjson);
            //watch.Stop();
            //Console.WriteLine("反序列化：{0}", watch.ElapsedMilliseconds);
            //TestResponse re = JsonHelper.Deserialize<TestResponse>("{\"IsError\":false,\"Data\":[{\"InterfaceName\":\"SOAFramework.Service.Server.DefaultService.DiscoverService\"}]}");
            #endregion

            #region custom wcf binding
            //string baseAddress = "Http://localhost/Service";
            //ServiceHost host = new WebServiceHost(typeof(SOAService), new Uri(baseAddress));
            //host.AddServiceEndpoint(typeof(IService), new BasicHttpBinding(), "soap");
            //WebHttpBinding webBinding = new WebHttpBinding();
            //webBinding.ContentTypeMapper = new MyRawMapper();
            //host.AddServiceEndpoint(typeof(IService), webBinding, "json").Behaviors.Add(new NewtonsoftJsonBehavior());
            //Console.WriteLine("Opening the host");
            //host.Open();

            //ChannelFactory<IService> factory = new ChannelFactory<IService>(new BasicHttpBinding(), new EndpointAddress(baseAddress + "/soap"));
            //IService proxy = factory.CreateChannel();
            //byte[] newdata;
            //TestClass c1 = new TestClass();
            //c1.a = "a";
            //List<TestClass> list1 = new List<TestClass>();
            //list1.Add(c1);
            //string strnewdata = JsonHelper.Serialize(list1);
            //newdata = Encoding.UTF8.GetBytes(strnewdata);
            //string newtestresult = HttpHelper.Post(@"http://localhost/Service/SOAFramework.Service.Server.DefaultService/DiscoverService", newdata);

            //Console.WriteLine("Now using the client formatter");
            //ChannelFactory<IService> newFactory = new ChannelFactory<IService>(webBinding, new EndpointAddress(baseAddress + "/json"));
            //newFactory.Endpoint.Behaviors.Add(new NewtonsoftJsonBehavior());
            //IService newProxy = newFactory.CreateChannel();

            //Console.WriteLine("Press ENTER to close");
            //Console.ReadLine();
            //host.Close();
            //Console.WriteLine("Host closed");
            #endregion

            #region wcf host
            WebServiceHost newhost = new WebServiceHost(typeof(SOAService));
            newhost.Open();
            newhost.Ping();
            #endregion

            #region zip tester
            string zip = "i am a string, to be zipped!";
            string zipped = ZipHelper.Zip(zip, System.Text.Encoding.Default);
            zip = ZipHelper.UnZip(zipped);
            #endregion

            #region orm
            //string abc = Model.Users.Mapping.ColumnsMapping["PK_UserID"].ToString();
            //a c = new a();
            //c.b = "haha ";
            //c.t = new a();
            //c.t.b = "aaaaaa";
            //string str1 = JsonHelper.Serialize(c);

            //c = JsonHelper.Deserialize<a>(str1);

            //FTPClient f = new FTPClient();
            //f.FtpUrl = "ftp://localhost/";
            //f.FileName = "ha.txt";
            //f.UserName = "remote";
            //f.Password = "liuxiao";
            //f.BufferSize = 10;
            //f.LocalFilePath = "e:";
            //f.Download();

            //sw.Stop();
            //Console.WriteLine("linq:" + sw.ElapsedTicks);
            //Model.Users objUser = new Model.Users();
            //Hashtable htArgs = new Hashtable();
            //htArgs["str"] = "ok";
            //WebServiceCaller wsCaller = new WebServiceCaller();
            //wsCaller.Action = "PostTest";
            //wsCaller.Args = htArgs;
            //wsCaller.Action = "GetTest";
            //wsCaller.WSUrl = @"http://localhost:1433/WebServicePool.asmx";
            //string strPost = wsCaller.SoapCall();
            //string strPost = SOAFramework.Common.HttpUtil.VisitWebPage_Post("http://localhost:1433/WebServicePool.asmx", "PostTest", "ok");
            //ClientRequest cr = new ClientRequest();

            //cr.Controller = "ScanRecordController";
            //cr.MethodName = "Query";
            //string strFileName = @"F:\TestOut.docx";
            //List<MethodArg> lstArg = new List<MethodArg>();
            //lstArg.Add(new MethodArg("there"));
            //cr.RequestData.MethodArgs = lstArg.ToArray();
            //cr.RequestType = WSDataType.JSON;
            //string str = cr.GetRequestString();
            //cr.ResponseType = WSDataType.JSON;
            //string strReturn = cr.SendRequest();
            //ServerResponse response = cr.GetResponse();
            //byte[] bytTemp = response.ResponseData;
            //Stream sw = File.Open(strFileName, FileMode.OpenOrCreate, FileAccess.Write);
            //sw.Write(bytTemp, 0, bytTemp.Length);
            //sw.Close();

            //Model.Users u = new Model.Users();
            //Model.Customer_AutoIncrease t = new Model.Customer_AutoIncrease();
            #endregion

            #region soa tester

            //testresult = HttpUtility.Get("http://localhost/Service/GetTest");

            argslist.Add(new SOAFramework.Service.SDK.Core.PostArgItem { Key = "url", Value = "http://localhost/" });
            //argslist.Add(new PostArgItem { Key = "usage", Value = "1.00" });
            strData = JsonHelper.Serialize("http://localhost/");
            //strData = "\"" + strData + "\"";
            data = System.Text.Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost:8082/Service/RegisterDispatcher/1", data);

            watch.Start();
            List<TestClass> listc = new List<TestClass>();
            //listc.Add(c);
            argslist.Clear();
            strData = JsonHelper.Serialize(argslist);
            data = System.Text.Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/Execute/SOAFramework.Service.Server.DefaultService/DiscoverService", data);
            testresult = ZipHelper.UnZip(testresult);
            watch.Stop();
            Console.WriteLine("发现服务测试耗时{0}", watch.ElapsedMilliseconds);


            watch.Restart();
            argslist.Clear();
            strData = JsonHelper.Serialize(argslist);
            data = System.Text.Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/Execute/SOAFramework.Service.Server.DefaultService/BigDataTest", data);
            watch.Stop();
            Console.WriteLine("大数据测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Restart();
            //download test
            string filename = "预付款类型批量导入.xls";
            testresult = HttpHelper.Get(@"http://localhost/Service/Download/" + filename);
            //testresult = ZipHelper.UnZip(testresult);
            testresult.ToFile("D:\\" + filename);
            watch.Stop();
            Console.WriteLine("下载测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Restart();
            //uploadtest
            string uploadFileName = "D:\\预付款类型批量导入.xls";
            FileInfo file = new FileInfo(uploadFileName);
            string fileString = file.FileToString();
            data = System.Text.Encoding.UTF8.GetBytes(fileString);
            testresult = HttpHelper.Post(@"http://localhost/Service/Upload/" + file.Name, data);
            watch.Stop();
            Console.WriteLine("上传测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Restart();
            int count = 10000;
            for (int i = 0; i < count; i++)
            {
                List<SOAFramework.Service.SDK.Core.PostArgItem> list = new List<SOAFramework.Service.SDK.Core.PostArgItem>();
                list.Add(new SOAFramework.Service.SDK.Core.PostArgItem { Key = "a", Value = JsonHelper.Serialize("hello world") });
                list.Add(new SOAFramework.Service.SDK.Core.PostArgItem { Key = "b", Value = JsonHelper.Serialize(new TestClass { a = "a", b = "b" }) });
                //list.Add(new PostArgItem { Key = "a", Value = "hello world" });
                //list.Add(new PostArgItem { Key = "b", Value = new TestClass { a = "a", b = "b" } });
                strData = JsonHelper.Serialize(list);
                data = System.Text.Encoding.UTF8.GetBytes(strData);
                //testresult = HttpHelper.Post(@"http://localhost/Service/Execute/SOAFramework.Service.Server.SOAService/Test", data);
                testresult = ZipHelper.UnZip(testresult);

                PerformanceRequest prequest = new PerformanceRequest();
                prequest.a = "hello world";
                prequest.b = new TestClass { a = "a", b = "b" };
                PerformanceResponse presponse = SDKFactory.Client.Execute(prequest);
            }
            watch.Stop();
            Console.WriteLine("{1}次测试耗时{0}", watch.ElapsedMilliseconds, count);
            #endregion

            #region sdk testing
            //TestRequest request = new TestRequest();
            //TestResponse reseponse = SDKFactory.Client.Execute(request);
            #endregion

            Console.ReadLine();
        }

        private static void Domain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {

        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = new AssemblyName(args.Name).Name;
            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string assemblyPath = Path.Combine(folderPath, fileName + ".dll");
            FileInfo file = new FileInfo(assemblyPath);
            Assembly ass = null;
            if (!file.Exists)
            {
                string newFileName = file.Directory + "\\Modules\\" + file.Name;
                if (File.Exists(newFileName))
                {
                    ass = Assembly.LoadFile(newFileName);
                    return ass;
                }
            }
            else
            {
                ass = Assembly.LoadFile(file.FullName);
                return ass;
            }
            return ass;
        }

        private void WriteFile(byte[] Stream, string FullFileName)
        {

        }
        #region fast sort
        public static void FastSort(List<long> SortInt, int StartIndex, int EndIndex)
        {
            //如果结束索引小于或等于开始索引，说明排序粒度已经最小，只有一个数字了
            if (EndIndex <= StartIndex)
            {
                return;
            }
            //初始比较值的索引
            int intIndex = StartIndex;
            //目标比较值的索引
            int intTargetIndex = EndIndex;
            //根据数组的宽度决定处理多少次
            for (int intLoopCount = 0; intLoopCount <= EndIndex - StartIndex; intLoopCount++)
            {
                //初始比较值索引在目标比较值索引左边时，初始比较值比目标比较值大，交换一下值，将较小值放在初始比较值左边
                if (SortInt[intIndex] > SortInt[intTargetIndex] && intIndex < intTargetIndex)
                {
                    //交换值
                    long intTempValue = SortInt[intIndex];
                    SortInt[intIndex] = SortInt[intTargetIndex];
                    SortInt[intTargetIndex] = intTempValue;
                    //交换索引
                    int intTempIndex = intIndex;
                    intIndex = intTargetIndex;
                    intTargetIndex = intTempIndex;
                }
                //初始比较值索引在目标比较值索引右边时，初始比较值比目标比较值小，交换一下，较小值放在初始比较值左边
                else if (SortInt[intIndex] < SortInt[intTargetIndex] && intIndex > intTargetIndex)
                {
                    //交换值
                    long intTempValue = SortInt[intIndex];
                    SortInt[intIndex] = SortInt[intTargetIndex];
                    SortInt[intTargetIndex] = intTempValue;
                    //交换索引
                    int intTempIndex = intIndex;
                    intIndex = intTargetIndex;
                    intTargetIndex = intTempIndex;
                }
                //目标比较值索引向初始比较值索引靠拢
                if (intIndex < intTargetIndex)
                {
                    intTargetIndex--;
                }
                else if (intIndex > intTargetIndex)
                {
                    intTargetIndex++;
                }
                else
                {
                    continue;
                }
            }
            int intLeftStartIndex = StartIndex;
            int intLeftEndIndex = intIndex;
            int intRightStartIndex = intIndex + 1;
            int intRightEndIndex = EndIndex;
            //将初始比较值左边的数组进行一次排序
            FastSort(SortInt, intLeftStartIndex, intLeftEndIndex);
            //将初始比较值右边的数组进行一次排序
            FastSort(SortInt, intRightStartIndex, intRightEndIndex);
        }
        #endregion
        public static void DoAction<T>(Action<T> action) where T : new()
        {
            T t = new T();
            action.Invoke(t);
        }

        public static R TestLinq<T, R>(Expression<Func<T, R>> ex)
            where T : new()
            where R : new()
        {
            T t = new T();
            ex.Compile().Invoke(t);
            R r = new R();
            return r;
        }

        public static T Select<T>(T t)
        {
            return t;
        }
    }

    public interface TestInterface
    {
        string str1 { get; set; }
    }

    public class TestInterfaceDerived1 : TestInterface
    {
        public string str1 { get; set; }

        public string str2 { get; set; }
    }

    public class TestInterfaceDerived2 : TestInterfaceDerived1
    {

    }

    [AOPClass(AttributeArea = AOPAttributeArea.Class)]
    public class Test : AOPClass
    {
        [AOPMethod(CustomAfterMethodName = "Go", CustomAspectMethoNameSpace = "Test.a")]
        public void Run(string a)
        {
            Console.WriteLine("run here " + a);
        }
    }

    public class a : DataContext
    {
        public a(string str) :
            base(str)
        { }
        public void Go()
        {
            Console.WriteLine("go here");
        }

        public string s { get; set; }
        public string b { get; set; }
        public a t { get; set; }
    }

    public class TestClass
    {
        public string a { get; set; }
        public string b { get; set; }

        public string d { get; set; }

        //public DateTime f { get; set; }

        public int g { get; set; }

        public List<TestClass> test { get; set; }

        public Dictionary<string, string> dic { get; set; }

        public DateTime dt { get; set; }

        public TestClass tc { get; set; }

        public DataTable table { get; set; }
    }
    
    [XmlRoot("request")]
    public class TestRequest : IRequest<TestResponse>
    {
        public string GetApi()
        {
            return "SOAFramework.Service.Server.DefaultService.DiscoverService";
        }

        public TestClass c { get; set; }
    }

    public class TestResponse : BaseResponse
    {
        public List<Dictionary<string, object>> Data { get; set; }
    }

    public class PerformanceRequest : IRequest<PerformanceResponse>
    {
        public string GetApi()
        {
            return "SOAFramework.Service.Server.SOAService.Test";
        }

        public string a { get; set; }

        public TestClass b { get; set; }
    }

    public class PerformanceResponse : BaseResponse
    {
        public TestClass Data { get; set; }
    }

    public class LogonResponse : BaseResponse
    {
        public string token { get; set; }
    }

    public class LogonRequest : BaseRequest<BaseResponse>
    {
        public override string GetApi()
        {
            return "RiskMgr.Api.LogonApi.Logon";
        }

        public string username { get; set; }

        public string password { get; set; }
    }

    public static class TestExtension
    {
        public static R TestSelect<T, R>(this IEnumerable<T> enu, Expression<Func<T, R>> ex)
            where T : new()
        {
            T t = new T();
            foreach (var a in enu)
            {

            }
            R r = Activator.CreateInstance<R>();

            return r;
        }

        public static void SetTest<T, TValue>(this IEnumerable<T> list, Expression<Func<T, TValue>> expression, TValue value)
        {

        }
    }
    public enum TestEnum
    {
        a = 1,
        b = 2,
    }

    
}
