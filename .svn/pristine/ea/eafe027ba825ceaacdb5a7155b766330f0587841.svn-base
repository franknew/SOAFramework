using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SOAFramework.Library.DAL;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
namespace UnitTest.Library
{
    [TestClass]
    public class DALTesting
    {
        static MongoCollection<MongoEntity> manager = null;

        [TestMethod]
        public void TestMongoInsert()
        {
            MongoEntity entity = new MongoEntity { Remark = "insert1" };
            manager.Insert(entity);
            Assert.IsNotNull(manager.FindOneById(entity.Id));
            var query = Query<MongoEntity>.EQ(t=>t.Id, entity.Id);
            manager.Remove(query);
        }

        [TestMethod]
        public void TestMongoUpdate()
        {
            manager.Update(Query<MongoEntity>.EQ(t => t.Remark, "hello"), Update<MongoEntity>.Set(t => t.Remark, "world"));
            Assert.AreEqual("world", manager.FindOne(Query<MongoEntity>.EQ(t => t.Remark, "world")).Remark);
        }

        [TestMethod]
        public void TestMongoRemove()
        {
            MongoEntity entity = new MongoEntity { Remark = "remove1" };
            manager.Insert(entity);
            manager.Remove(Query<MongoEntity>.EQ(t => t.Id, entity.Id));
            Assert.IsNull(manager.FindOneById(entity.Id));
            
        }

        [TestMethod]
        public void TestMongoQuery()
        {
            Assert.IsNotNull(manager.FindOne(Query<MongoEntity>.EQ(t => t.Remark, "query1")));
        }

        [TestInitialize()]
        public void Init()
        {
            MongoDBHelper helper = new MongoDBHelper("MongoDBTesting", "mongodb://frank:frank@localhost");
            manager = helper.GetDataManager<MongoEntity>();
            MongoEntity entity = new MongoEntity { Remark = "hello" };
            MongoEntity queryentity = new MongoEntity { Remark = "query1" };
            manager.Insert(entity);
            manager.Insert(queryentity);
        }
        [TestCleanup]
        public void Clearnup()
        {
            manager.RemoveAll();
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
    }

    public class MongoEntity : BaseMongoEntity
    {
        public string Remark { get; set; }
    }
}
