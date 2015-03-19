using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Dynamic;
using System.Reflection;
using MongoDB.Bson.Serialization;
using System.Configuration;

namespace SOAFramework.Library.DAL
{
    public class MongoDBHelper
    {
        public MongoDBHelper(string database, string connectionstring = null)
        {
            dataBase = database;
            if (string.IsNullOrEmpty(connectionstring) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                connectionstring = ConfigurationManager.AppSettings["ConnectionString"];
            }
            connectionString = connectionstring;
            MongoClient client = new MongoClient(connectionString);
            mongoDataBaseManager = client.GetDatabase(database);
        }

        private IMongoDatabase mongoDataBaseManager = null;

        private string dataBase = "";

        public string DataBase
        {
            get { return dataBase; }
        }

        private string connectionString = "";

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public IMongoCollection<T> GetDataManager<T>() where T : BaseNoSQLEntity
        {
            if (mongoDataBaseManager == null)
            {
                return null;
            }
            IMongoCollection<T> mongoDataBase = mongoDataBaseManager.GetCollection<T>(typeof(T).Name);
            return mongoDataBase;
        }
    }
}
