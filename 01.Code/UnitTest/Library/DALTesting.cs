using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SOAFramework.Library.DAL;
using MongoDB.Driver;
namespace UnitTest.Library
{
    [TestClass]
    public class DALTesting
    {
        static IMongoCollection<MongoEntity> manager = null;

        [TestMethod]
        public void TestMongoInsert()
        {
            MongoEntity entity = new MongoEntity { Remark = "insert1" };
            manager.InsertOneAsync(entity);
            Assert.IsNotNull(manager.FindAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Id, entity.Id)).Result);
            manager.FindOneAndDeleteAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Id, entity.Id));
        }

        [TestMethod]
        public void TestMongoUpdate()
        {
            manager.FindOneAndUpdateAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Remark, "hello"), new UpdateDefinitionBuilder<MongoEntity>().Set(t => t.Remark, "world"));
            Assert.AreEqual("world", manager.FindAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Remark, "world")));
        }

        [TestMethod]
        public void TestMongoRemove()
        {
            MongoEntity entity = new MongoEntity { Remark = "remove1" };
            manager.InsertOneAsync(entity);
            manager.FindOneAndDeleteAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Id, entity.Id));
            Assert.IsNull(manager.FindAsync(new FilterDefinitionBuilder<MongoEntity>().Eq(t => t.Id, entity.Id)));

        }

        [TestMethod]
        public void TestMongoQuery()
        {
            FilterDefinitionBuilder<MongoEntity> builder = new FilterDefinitionBuilder<MongoEntity>();
            builder.Eq(t => t.Remark, "query1");
            Assert.IsNotNull(manager.Find(builder.Eq(t => t.Remark, "query1")).FirstOrDefaultAsync());
            //Assert.IsNotNull(manager.FindOne(Query<MongoEntity>.EQ(t=>t.Id, BsonValue)));
            //Assert.IsNotNull(manager.FindOne(Query.And(Query.Not(Query.EQ("id", null)), Query<MongoEntity>.EQ(t => t.Remark, "query1"))));
        }

        [TestInitialize()]
        public void Init()
        {
            MongoDBHelper helper = new MongoDBHelper("MongoDBTesting", "mongodb://frank:frank@localhost");
            manager = helper.GetDataManager<MongoEntity>();
            MongoEntity entity = new MongoEntity { Remark = "hello" };
            MongoEntity queryentity = new MongoEntity { Remark = "query1" };
            manager.InsertOneAsync(entity);
            manager.InsertOneAsync(queryentity);
        }
        [TestCleanup]
        public void Clearnup()
        {
            manager.DeleteManyAsync("{}");
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
