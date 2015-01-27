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
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Test
{
    class Program
    {
        public delegate void dl();
        static void Main(string[] args)
        {
            TestClass cc = new TestClass();
            Update<TestClass>.Set(t => t.a, "");
            List<TestClass> lists = new List<TestClass>();
            lists.Add(new TestClass { a = "a" });
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
            string zipped = ZipHelper.Zip(zip);
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
            TestRequest request = new TestRequest();
            TestResponse reseponse = SDKFactory.Client.Execute(request);
            #endregion

            Console.ReadLine();
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

    [AOPClass(AttributeArea = AOPAttributeArea.Class)]
    public class Test : AOPClass
    {
        [AOPMethod(CustomAfterMethodName = "Go", CustomAspectMethoNameSpace = "Test.a")]
        public void Run(string a)
        {
            Console.WriteLine("run here " + a);
        }
    }

    public class a
    {
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
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public string GetApi()
        {
            return "SOAFramework.Service.Server.DefaultService.DiscoverService";
        }
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
    }

}
