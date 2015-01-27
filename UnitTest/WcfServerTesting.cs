using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel.Web;
using SOAFramework.Service.Server;
using SOAFramework.Library;
using System.Collections.Generic;
using SOAFramework.Service.SDK.Core;
using System.Reflection;
using System.IO;
using SOAFramework.Service.Core.Model;

namespace UnitTest
{
    [TestClass]
    public class WcfServerTesting
    {
        private WebServiceHost host = new WebServiceHost(typeof(SOAService));

        [TestCleanup]
        public void CleanUp()
        {
            host.Close();
        }

        [TestInitialize]
        public void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            host.Open();
        }

        [TestMethod]
        public void WcfCallTesting()
        {
            List<PostArgItem> argslist = new List<PostArgItem>();
            string strData = JsonHelper.Serialize(argslist);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strData);
            string testresult = HttpHelper.Post(@"http://localhost/Service/SOAFramework.Service.Server.DefaultService/DiscoverService", data);
            testresult = ZipHelper.UnZip(testresult);
            Assert.IsFalse(string.IsNullOrEmpty(testresult));
        }

        [TestMethod]
        public void SdkTesting()
        {
            TestRequest request = new TestRequest();
            TestResponse reseponse = SDKFactory.Client.Execute(request);
            Assert.IsNotNull(reseponse);
            Assert.IsFalse(reseponse.IsError);
            Assert.IsTrue(reseponse.Data.Count > 0);
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
}
