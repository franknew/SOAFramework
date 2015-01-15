using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        /// <summary>
        /// 产生一个helper进行查询相关的操作
        /// </summary>
        /// <returns></returns>
        public static TableModelHelper<T> CreateHelper()
        {
            TableModelHelper<T> objHelper = new TableModelHelper<T>(_dt_ClassDef, _cad_TableDef);
            return objHelper;
        }
    }
}