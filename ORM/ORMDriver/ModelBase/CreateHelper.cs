using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SOAFramework.ORM.ORMDriver
{
    partial class TableModelBase<T> where T : new()
    {
        /// <summary>
        /// ����һ��helper���в�ѯ��صĲ���
        /// </summary>
        /// <returns></returns>
        public static TableModelHelper<T> CreateHelper()
        {
            TableModelHelper<T> objHelper = new TableModelHelper<T>(_dt_ClassDef, _cad_TableDef);
            return objHelper;
        }
    }
}