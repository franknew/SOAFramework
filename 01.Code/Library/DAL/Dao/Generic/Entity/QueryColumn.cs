using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public class QueryColumn: MappingColumn
    {
        public QueryColumn(string name, OperationTypeEnum operation, object value)
        {
            this.Name = name;
            this.Operation = OperationTypeConverter.Convert(operation);
            this.Value = value;
        }

        public QueryColumn(string name, OperationTypeEnum operation, List<Object> values)
        {
            this.Name = name;
            this.Operation = OperationTypeConverter.Convert(operation);
            this.Values = values;
        }

        public string Operation { get; set; }
        public List<Object> Values { get; set; }
    }
}
