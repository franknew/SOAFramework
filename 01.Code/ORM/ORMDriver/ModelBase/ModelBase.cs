using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using SOAFramework.ORM.Common;
using SOAFramework.ORM.Mapping;


namespace SOAFramework.ORM.ORMDriver
{
    public partial class TableModelBase<T> where T : new()
    {
        private static readonly TableMapping _tm_Mapping = TableMapping.CreateInstance(typeof(T));

        static TableModelBase()
        {
            T objTemp = new T();
            _dt_ClassDef = Reflection.GetClassDef<T>(objTemp);
            _cad_TableDef = _tm_Mapping;
            //Reflection.GetColumnsDef(_dic_ClassDef, objTemp, _cad_TableDef.TableName);
        }

        public static TableMapping Mapping
        {
            get
            {
                return _tm_Mapping;
            }
        }

        public TableModelBase()
        {
            _bl_IsNew = true;
            //CopyValue();
        }
        /// <summary>
        /// ����һ�ݱ�����������ƥ�䣬�Ա���£��û�����Ҫʹ�ø÷���
        /// </summary>
        public void CopyValue()
        {
            _dt_ValueCopy = Reflection.GetClassValues(this, _dt_ClassDef);
        }
    }

}