using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library.DAL;
using Norm.Collections;
using Norm.BSON;
using Norm;
namespace UnitTest.Library
{
    [TestClass]
    public class DALTesting
    {
        static IMongoCollection<MongoEntity> manager = null;

        [TestMethod]
        public void TestMongoInsert()
        {
            MongoEntity entity = new MongoEntity { 备注 = "insert1" };
            manager.Insert(new MongoEntity[] { entity });
            var query = new Expando();
            query["_id"] = entity.Id;
            Assert.IsNotNull(manager.FindOne(query));
            manager.Delete(query);
        }

        [TestMethod]
        public void TestMongoUpdate()
        {
            manager.UpdateOne(new { 备注 = "hello" }, new { 备注 = M.Set("world") });
            string remark = manager.FindOne(new { 备注 = "world" }).备注;
            Assert.AreEqual("world", remark);
        }

        [TestMethod]
        public void TestMongoRemove()
        {
            MongoEntity entity = new MongoEntity { 备注 = "remove1" };
            manager.Insert(entity);
            manager.Delete(new { 备注 = "remove1" });
            Assert.IsNull(manager.FindOne(new { 备注 = "remove1" }));

        }

        [TestMethod]
        public void TestMongoQuery()
        {
            Assert.IsNotNull(manager.FindOne(new { 备注 = "query1" }));
            //Assert.IsNotNull(manager.FindOne(Query<MongoEntity>.EQ(t=>t.Id, BsonValue)));
            //Assert.IsNotNull(manager.FindOne(Query.And(Query.Not(Query.EQ("id", null)), Query<MongoEntity>.EQ(t => t.Remark, "query1"))));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [TestMethod]
        public void TestDocumentEQQuery()
        {
            var query = new Expando();
            query["备注"] = "query1";
            Assert.IsNotNull(manager.FindOne(query));
        }

        [TestInitialize()]
        public void Init()
        {
            MongoDBHelper helper = new MongoDBHelper("MongoDBTesting", "mongodb://frank:frank@localhost");
            manager = helper.GetDataManager<MongoEntity>();
            MongoEntity entity = new MongoEntity { 备注 = "hello" };
            MongoEntity queryentity = new MongoEntity { 备注 = "query1" };
            manager.Insert(new MongoEntity[] { entity, queryentity });
        }

        [TestCleanup]
        public void Clearnup()
        {
            manager.Delete(new { });
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
        public string 备注 { get; set; }
    }
}
