using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public interface IGenericOperationInner
    {
        DataTable Query(QueryEntity query, ISqlMapper mapper);
    }
}
