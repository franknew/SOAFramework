using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseMongoEntity : BaseNoSQLEntity
    {
        public override string Id
        {
            get
            {
                return ObjectId.ToString();
            }
            set
            {
                ObjectId = new ObjectId(value);
            }
        }

        public ObjectId ObjectId { get; set; }
    }
}
