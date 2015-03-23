
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm.Attributes;
using Norm;

namespace SOAFramework.Library.DAL
{
    public class BaseMongoEntity : BaseNoSQLEntity
    {
        public BaseMongoEntity()
        {
            Id = ObjectId.NewObjectId();
        }

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

        public ObjectId Id { get; private set; }
    }
}
