using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;
using System.Configuration;
using Norm.Collections;
using Norm.BSON;
using Norm;

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
        }

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
            IMongoCollection<T> mongoDataBase = Mongo.Create(connectionString).GetCollection<T>(typeof(T).Name);
            return mongoDataBase;
        }
    }
}
