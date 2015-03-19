
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public interface IDataManager<T> where T : BaseNoSQLEntity
    {
        bool Add(T t);
    }
}
