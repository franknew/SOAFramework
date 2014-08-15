using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.ORM;
using SOAFramework.ORM.Common;

namespace UnitTest
{
    /// <summary>
    /// ORMTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ORMTest
    {
        public ORMTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
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

        //[TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: 在此处添加测试逻辑
            //
        }

        [TestMethod]
        public void TestAdd()
        {
            Model.Customer_AutoIncrease Customer = new Model.Customer_AutoIncrease();
            Customer.Name = "test1";
            Customer.Age = "10";
            Customer.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm");
            Customer.Save();

            Assert.IsNotNull(Model.Customer_AutoIncrease.CreateHelper().IsExists("Name='test1'"));
        }

        [TestMethod]
        public void TestUpdate()
        {
            Model.Customer_AutoIncrease Customer = new Model.Customer_AutoIncrease();
            Customer.Name = "test2";
            Customer.Age = "10";
            Customer.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm");
            Customer.ID = Customer.Save().ToString();

            Customer.Age = "20";
            Customer.Update();

            Model.Customer_AutoIncrease CustomerTemp = Model.Customer_AutoIncrease.CreateHelper().Where("Name='Test2'").GetModel();
            Assert.AreEqual<string>("20", Customer.Age);
        }

        //[TestMethod]
        public void TestAddTrans()
        {
            Transaction objTrans = new Transaction();
            objTrans.Begin();
            Model.Customer_AutoIncrease Customer = new Model.Customer_AutoIncrease();
            Customer.Name = "test3";
            Customer.Age = "10";
            Customer.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm");
            Customer.Save(objTrans);
            objTrans.RollBack();

            Assert.IsNull(Model.Customer_AutoIncrease.CreateHelper().IsExists("Name='test3'"));
        }
    }
}
