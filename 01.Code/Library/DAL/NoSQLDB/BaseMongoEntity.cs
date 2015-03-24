
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm.Attributes;
using Norm;

namespace SOAFramework.Library.DAL
{
    public class BaseMongoEntity<T> : BaseNoSQLEntity
        where T : BaseNoSQLEntity
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

        public ObjectId Id { get; private set; }

        public T Create()
        {
            T t = Activator.CreateInstance<T>();
            t.Id_string = ObjectId.NewObjectId();
            return t;
        }
    }
}
