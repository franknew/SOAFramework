using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public class QueryEntity
    {
        public string TableName { get; set; }
        public Condition Condition { get; set; }
        public List<string> Columns { get; set; }
    }
}
