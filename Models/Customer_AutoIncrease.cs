using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.ORM.ORMDriver;
using SOAFramework.ORM.Mapping;

namespace Model
{
    [TableMapping(TableName="Customer_AutoIncrease")]
    public class Customer_AutoIncrease : TableModelBase<Customer_AutoIncrease>
    {
        [ColumnMapping(AttrType=typeof(int), IsAutoIncrease=true, IsPrimaryKey=true)]
        public string ID
        { get; set; }

        public string Name
        { get; set; }

        public string Age
        { get; set; }

        public string CreateTime
        { get; set; }
    }
}
