using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using SOAFramework.ORM.Mapping;

namespace SOAFramework.ORM.ORMDriver
{
    public partial class TableModelBase<T> where T : new()
    {
        private static DataTable _dt_ClassDef;
        private static TableMapping _cad_TableDef;
        private static Dictionary<string, List<ColumnMapping>> _dic_ClassDef = new Dictionary<string, List<ColumnMapping>>();

        public string DBExString = "";
        public bool _bl_IsNew = true;
        private DataTable _dt_ValueCopy;
    }
}
