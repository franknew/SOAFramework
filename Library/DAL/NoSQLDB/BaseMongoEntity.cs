using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseMongoEntity : BaseNoSQLEntity
    {
        public override string Id_string
        {
            get
            {
                string id = "";
                if (Id != null)
                {
                    id = Id.ToString();
                }
                return id;
            }
            set
            {
                Id = new ObjectId(value);
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }
    }
}
