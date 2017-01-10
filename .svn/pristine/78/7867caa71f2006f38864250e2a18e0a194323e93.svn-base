using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.ORM.ORMDriver;
using SOAFramework.ORM.Mapping;

namespace Models
{
    [TableMapping(DBOwner = "dbo", TableName = "上货扫描记录")]
    public class 上货扫描记录 : TableModelBase<上货扫描记录>
    {
        [ColumnMapping(AttrType = typeof(int), IsPrimaryKey = true, IsAutoIncrease = true)]
        public int _Identify { get; set; }

        public bool _Locked { get; set; }

        public int _SortKey { get; set; }

        public string 车次 { get; set; }

        public string 货号 { get; set; }

        public string 扫描人 { get; set; }

        public string 扫描时间 { get; set; }
    }
}
