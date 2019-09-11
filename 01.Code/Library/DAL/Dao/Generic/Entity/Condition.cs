using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public class Condition
    {
        private List<QueryColumn> columns = new List<QueryColumn>();
        public List<QueryColumn> Columns { get => columns; set => columns = value; }
    }
}
