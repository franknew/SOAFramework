using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public static class LinqExtension
    {
        public static IMongoQuery ToMongoQuery<T>(this Expression<Func<T, bool>> query) where T : BaseNoSQLEntity
        {
            IMongoQuery mongoQuery = (query as MongoQueryable<T>).GetMongoQuery();
            return mongoQuery;
        }
    }
}
