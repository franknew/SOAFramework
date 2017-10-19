using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroService.Library;
using CodeSmith.Engine.Json.Linq;
using Pac.Api.Dataobject.Request.TMS_CREATE_ORDER_ONLINE_NOTIFY;
using SOAFramework.Library;
using Pac.Api.Dataobject.Response.TMS_CREATE_ORDER_ONLINE_NOTIFY;
using Pac.Api;
using Pac.Api.Dataobject.Request.TMS_WAYBILL_SUBSCRIPTION_APPLY;
using Pac.Api.Dataobject.Request.TMS_UPDATE_ORDER_OFFLINE_NOTIFY;
using Pac.Api.Dataobject.Request.TRACE_INFO_QUERY;
using Pac.Api.Dataobject.Request.TMS_WAYBILL_DETAIL_SEND_BATCH;

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

        //[ClassInitialize]
        //public void Init()
        //{
        //}

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

            TmsCreateOrderOnlineNotifyRequest request = new TmsCreateOrderOnlineNotifyRequest
            {
                logisticsId = "LBX1023892031",
                cpCode = "SF",
                dicCode = "DCP00098765",
                mailNo = "800090004703",
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
            XmlSerializor x = new XmlSerializor();
            string postData = string.Format("logistics_interface={0}&AppKey={1}", x.Serialize(request), "F296835E2F1708BC");
           var xx= SerializeHelper.Desrialize(x.Serialize(request),typeof(TmsCreateOrderOnlineNotifyRequest));
           // var result = HttpHelper.Post("http://183.62.238.70:9081/TPI_Api/Unitop.TPI.Service.Waybill.Subscribe", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");


            //  Assert.IsNotNull(result);
            // TmsCreateOrderOnlineNotifyResponse response = new JsonSerializor().Deserialize<TmsCreateOrderOnlineNotifyResponse>(result);
            // Assert.IsNotNull(response);
        }

        /// <summary>
        /// 电子面单申请
        /// </summary>
        [TestMethod]
        public void TestTPI_Subscribe_Apply()
        {

            TmsWaybillSubscriptionApplyRequest request = new TmsWaybillSubscriptionApplyRequest
            {
                branchAddress = new BranchAddress
                {
                    branchAddressId = 12345,
                    provinceName = "浙江省",
                    cityName = "杭州市",
                    areaName = "余杭",
                    townName = "余杭街道",
                    detailAddress = "狮山路11号"
                },
                branchCode = "80302282",
                sellerId = 123459,
                sellerNick = "微软旗舰店",
                contactPhone = "13827229088",
                cpCode = "YTO",
                customerCode = "28020380",
                subscriptionId = DateTime.Now.ToString("yyyyMMddHHmmss")

            };

            string postData = string.Format("logistics_interface={0}&AppKey={1}", new JsonSerializor().Serialize(request), "F296835E2F1708BC");

            var result = HttpHelper.Post("http://127.0.0.1:9080/TPI/Unitop.TPI.Service.Waybill.SubscriptionApply", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");

            Assert.IsNotNull(result);
            TmsCreateOrderOnlineNotifyResponse response = new JsonSerializor().Deserialize<TmsCreateOrderOnlineNotifyResponse>(result);
            Assert.IsNotNull(response);
        }

        /// <summary>
        /// 快递离线下发接口
        /// </summary>
        [TestMethod]
        public void Test_TMS_UPDATE_ORDER_OFFLINE_NOTIFY()
        {

            var request = new TmsUpdateOrderOfflineNotifyRequest
            {
                logisticsId = "LP10238920028",
                cpCode = "LTC",
                mailNo = "880227670409",
                packageWeight = "10050.5",
                packageVolume = "2050.6",
                status = "TMS_ORDER_RECEIVED"

            };

            string postData = string.Format("logistics_interface={0}&AppKey={1}", new JsonSerializor().Serialize(request), "F296835E2F1708BC");

            var result = HttpHelper.Post("http://127.0.0.1:9080/TPI/Unitop.TPI.Service.Waybill.UpdateOrderOfflineNotify", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");

        }

        /// <summary>
        /// 物流跟踪详情查询接口
        /// </summary>
        [TestMethod]
        public void Test_Trace_Info_Query()
        {

            var request = new TraceInfoQueryRequest
            {
                cpCode = "LTC",
                mailNos = new List<string>
                    {
                        "212677403088"
                    }

            };

            string postData = string.Format("logistics_interface={0}&AppKey={1}", new JsonSerializor().Serialize(request), "F296835E2F1708BC");

            var result = HttpHelper.Post("http://127.0.0.1:9080/TPI/Unitop.TPI.Service.Waybill.TraceInfoQuery", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");

           // Assert.IsTrue(result.Contains("无效单号"));
        }

        /// <summary>
        /// 面单信息推送
        /// </summary>
        [TestMethod]
        public void Test_TMS_WAYBILL_DETAIL_SEND_BATCH()
        {
            TmsWaybillDetailSendBatchRequest request = new TmsWaybillDetailSendBatchRequest();
            request.branchCode = "0020";
            request.waybillInfoList = new List<WaybillInfo>
            {
                new WaybillInfo
                {
                     sellerId=123456,
                     waybillCode="880211892117",
                     consigneeName="王小狗",
                     consigneePhone="123456",
                     consigneeMobile="13128888888",
                     sendAddress=new WaybillAddress
                     {
                         provinceName="浙江省",
                         cityName="杭州市",
                         areaName="余杭区",
                         townName="余杭街道",
                         detailAddress="文一西路969号阿里巴巴西溪园区3号楼小邮局"
                     },
                     consigneeAddress=new WaybillAddress
                     {
                         provinceName="浙江省",
                         cityName="杭州市",
                         areaName="余杭区",
                         townName="余杭街道",
                         detailAddress="文一西路969号阿里巴巴西溪园区3号楼小邮局"
                     },
                     status=2,
                     senderPhone="123456",
                     senderMobile="13120101011",
                     senderName="王小猪",
                     itemList=new List<SpecialItem>
                     {
                         new SpecialItem
                         {
                              name="蓝牙耳机",
                              count=1
                         },
                          new SpecialItem
                         {
                              name="苹果电脑",
                              count=1
                         }
                     },
                     weight=1000,
                     volume=1000,
                     createTime=DateTime.Now,
                     segmentCode="NORMAL"
                }
            };

            string postData = string.Format("logistics_interface={0}&AppKey={1}", new JsonSerializor().Serialize(request), "F296835E2F1708BC");

            var result = HttpHelper.Post("http://127.0.0.1:80/TPI/Unitop.TPI.Service.Waybill.WaybillDetailSendBatch", Encoding.UTF8.GetBytes(postData), -1, "application/x-www-form-urlencoded");

        }


    }
}
