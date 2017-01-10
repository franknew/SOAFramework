﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroService.Library;
using CodeSmith.Engine.Json.Linq;
using Pac.Api.Dataobject.Request.TMS_CREATE_ORDER_ONLINE_NOTIFY;
using SOAFramework.Library;
using Pac.Api.Dataobject.Response.TMS_CREATE_ORDER_ONLINE_NOTIFY;
using Pac.Api;

namespace UnitTest.Library
{
    /// <summary>
    /// MicroServiceTesting 的摘要说明
    /// </summary>
    [TestClass]
    public class MicroServiceTesting
    {
        public MicroServiceTesting()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestUrlJsonDeseriral()
        {
            TestClass c = new TestClass
            {
                a = "a",
                b = "b",
                f = new DateTime(2016,4,12),
                test = new List<TestClass>
                {
                    new TestClass { a = "hello", b = "xx" }
                }
            };
            //string json = "{\"a\":\"a\", \"b\":10,\"f\":\"2016-12-4 00:00:00\"},\"test\":[{\"a\":\"hello\",\"b\":\"xx\"}]}";
            string cjson = JsonHelper.Serialize(c);
            string code = string.Format("TestClass={0}", cjson);
            ISerializable serial = new UrlSerializor();
            var dic = serial.Deserialize<Dictionary<string, object>>(code);
            //var testclass = (dic["TestClass"] as JToken).ToObject<TestClass>();
            Assert.IsNotNull(dic);
            //Assert.IsNotNull(testclass);
            //Assert.AreEqual(testclass.a, "a");
            //Assert.AreEqual(testclass.b, 10);
            //Assert.AreEqual(testclass.f, new DateTime(2016, 12, 4));
        }

        [TestMethod]
        public void TestTPI_Waybill_Subscribe()
        {
            CommonRequest<TmsCreateOrderOnlineNotifyRequest> commonRequest = new CommonRequest<TmsCreateOrderOnlineNotifyRequest>();
            TmsCreateOrderOnlineNotifyRequest request = new TmsCreateOrderOnlineNotifyRequest
            {
                logisticsId = "LBX1023892031",
                cpCode = "SF",
                dicCode = "DCP00098765",
                mailNo = "20140804048802",
                orderCreateTime = "2014-07-28 10:05:34",
                orderBizType = "1",
                orderSource = "TB",
                serviceFlag = "1,2",
                tradeNo = "22227880099",
                storeDeliveryTime = Convert.ToDateTime("2014-07-30 14:00:00"),
                packageInfo = new PackageInfo
                {
                    packageWeight = "1000.5",
                    packageVolume = "2050.6",
                    packageWidth = "50",
                    packageHeight = "50",
                    packageLength = "50",
                    itemList = new List<ItemInfo> { new ItemInfo { itemName = "毛衣", itemNum = 1, insuredAmount = 199900, extendFields = "is_precious:1" } }
                },
                sender = new Sender
                {
                    name = "刘某",
                    phone = "0571-84292224",
                    mobile = "13989203824",
                    province = "浙江",
                    address = "浙江省杭州市余杭区文一西路823号",
                    storeCode = "YS001",
                    customerNo = "JD007689"
                },
                receiver = new Receiver
                {
                    name = "李某",
                    phone = "0571-84292224",
                    mobile = "13989203824",
                    province = "四川",
                    address = "浙江省杭州市余杭区文一西路823号",
                    receiverTown = "文一路",
                    receiverDivisionId = "67991"
                }
            };
            commonRequest.request = request;
            string postData = string.Format("logistics_interface={0}&AppKey={1}", new JsonSerializor().Serialize(commonRequest), "F296835E2F1708BC");

            var result = HttpHelper.Post("http://127.0.0.1:9080/TPI/Unitop.TPI.Service.Waybill.Subscribe", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");
           
            Assert.IsNotNull(result);
            TmsCreateOrderOnlineNotifyResponse response = new JsonSerializor().Deserialize<TmsCreateOrderOnlineNotifyResponse>(result);
            Assert.IsNotNull(response);
        }
    }
}
