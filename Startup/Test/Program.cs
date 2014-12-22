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
using SOAFramework.Service.Model;
using SOAFramework.Library.Zip;

namespace Test
{
    class Program
    {
        public delegate void dl();
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
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
            #endregion

            #region wcf host
            //WebServiceHost host = new WebServiceHost(typeof(SOAService));
            //host.Open();
            #endregion

            #region zip tester
            string zip = "i am a string, to be zipped!";
            string zipped = ZipHelper.Zip(zip);
            zip = ZipHelper.UnZip(zipped);
            #endregion

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

            #region soa tester
            string strData = "";
            byte[] data = null;
            string testresult = "";

            //testresult = HttpUtility.Get("http://localhost/Service/GetTest");

            TestClass c = new TestClass();
            c.a = "111";
            strData = JsonHelper.Serialize(c);
            //strData = "\"" + strData + "\"";
            data = Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/Test", data);

            watch.Start();
            List<PostArgItem> argslist = new List<PostArgItem>();
            strData = JsonHelper.Serialize(argslist);
            data = Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/SOAFramework.Service.Server.DefaultService/DiscoverService", data);
            testresult = ZipHelper.UnZip(testresult);
            watch.Stop();
            Console.WriteLine("发现服务测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Start();
            dynamic d = JsonHelper.Deserialize<dynamic>(testresult);
            strData = JsonHelper.Serialize(argslist);
            data = Encoding.UTF8.GetBytes(strData);
            testresult = HttpHelper.Post(@"http://localhost/Service/SOAFramework.Service.Server.DefaultService/BigDataTest", data);
            watch.Stop();
            Console.WriteLine("大数据测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Start();
            //download test
            string filename = "预付款类型批量导入.xls";
            testresult = HttpHelper.Get(@"http://localhost/Service/Download/" + filename);
            //testresult = ZipHelper.UnZip(testresult);
            testresult.ToFile("D:\\" + filename);
            watch.Stop();
            Console.WriteLine("下载测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Start();
            //uploadtest
            string uploadFileName = "D:\\预付款类型批量导入.xls";
            FileInfo file = new FileInfo(uploadFileName);
            string fileString = file.FileToString();
            data = Encoding.UTF8.GetBytes(fileString);
            testresult = HttpHelper.Post(@"http://localhost/Service/Upload/" + file.Name, data);
            watch.Stop();
            Console.WriteLine("上传测试耗时{0}", watch.ElapsedMilliseconds);

            watch.Start();
            int count = 10000;
            for (int i = 0; i < count; i++)
            {
                List<PostArgItem> list = new List<PostArgItem>();
                list.Add(new PostArgItem { Key = "str", Value = "a1" });
                list.Add(new PostArgItem { Key = "str1", Value = "a2" });
                strData = JsonHelper.Serialize(list);
                data = Encoding.UTF8.GetBytes(strData);
                testresult = HttpHelper.Post(@"http://localhost/Service/SOAFramework.Service.Server.Test/TestMethod", data);
                testresult = ZipHelper.UnZip(testresult);
                //SOAFramework.Service.Model.ServerResponse response = JsonHelper.Deserialize<SOAFramework.Service.Model.ServerResponse>(testresult);
            }
            watch.Stop();
            Console.WriteLine("{1}次测试耗时{0}", watch.ElapsedMilliseconds, count);
            #endregion

            Console.ReadLine();
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

}
