using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library.Cache;
using System.Runtime.Caching;
using System.Threading;

namespace UnitTest.Library
{
    /// <summary>
    /// CacheTesting 的摘要说明
    /// </summary>
    [TestClass]
    public class CacheTesting
    {
        private ICache cache = CacheFactory.Create();

        public CacheTesting()
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

        [TestInitialize]
        public void Init()
        {
            //cache.AddItem(new CacheItem("UnitTest1", "hello"), 10);
        }

        [TestCleanup]
        public void CleanUp()
        {
            cache.DelItem("UnitTest1");
        }

        [TestMethod]
        public void TestAddCache()
        {
                cache.AddItem("UnitTest2", "hello", 10);
                var item = cache.GetItem<string>("UnitTest2");
                Assert.AreEqual("hello", item);
                cache.DelItem("UnitTest2");

                cache.AddItem("UnitTest3", "hello", 1);
                Thread.Sleep(2000);
                item = cache.GetItem<string>("UnitTest3");
                Assert.IsNull(item);
        }

        [TestMethod]
        public void TestRemoveCache()
        {
            cache.AddItem("UnitTest2", "hello", 10);
            cache.DelItem("UnitTest2");
            var item = cache.GetItem<string>("UnitTest2");
            Assert.IsNull(item);
        }

        [TestMethod]
        public void TestGetCache()
        {
            var item = cache.GetItem<string>("UnitTest1");
            Assert.AreEqual("UnitTest1", item);
        }

        [TestMethod]
        public void TestUpdateCache()
        {
            var item = cache.GetItem<string>("UnitTest1");
            item = "hello world";
            cache.UpdateItem("UnitTest1", item, -1);
            item = cache.GetItem<string>("UnitTest1");
            Assert.AreEqual("hello world", item);
        }
    }
}
