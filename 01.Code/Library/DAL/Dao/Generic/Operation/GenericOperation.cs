using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public class GenericOperation : IGenericOperationInner
    {
        public DataTable Query(QueryEntity query, ISqlMapper mapper = null)
        {
            DataTable dataTable = new DataTable();
            if (mapper == null)
            {
                mapper = Mapper.Instance();
            }
            var list = mapper.QueryForList<IDictionary<string, object>>("SOAFramework.Library.DAL.Generic.Query", query);
            dataTable = list.ToDataTable();
            return dataTable;
        }
    }
}
