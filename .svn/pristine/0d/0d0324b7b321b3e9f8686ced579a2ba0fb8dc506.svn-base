using System;
using System.Collections.Generic;
using System.Text;

using SOAFramework.ORM.Common;
using System.Reflection;

namespace SOAFramework.ORM.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMapping : System.Attribute
    {
        #region attributes
        //是否主键
        private bool _bl_IsPrimaryKey = false;
        //字段类型
        private Type _typ_AttrType = null;
        //字段长度
        private int _int_Lenght = -1;
        //是否自增长
        private bool _bl_IsAutoIncrease = false;

        private PrimaryKeyGenerater _pkg_PrimaryKeyType;
        #endregion

        #region properties
        public bool IsPrimaryKey
        {
            set { _bl_IsPrimaryKey = value; }
            get { return _bl_IsPrimaryKey; }
        }

        public Type AttrType
        {
            set { _typ_AttrType = value; }
            get { return _typ_AttrType; }
        }

        public int Lenght
        {
            set { _int_Lenght = value; }
            get { return _int_Lenght; }
        }

        public bool IsAutoIncrease
        {
            set { _bl_IsAutoIncrease = value; }
            get { return _bl_IsAutoIncrease; }
        }

        public PrimaryKeyGenerater PrimaryKeyType
        {
            set { _pkg_PrimaryKeyType = value; }
            get { return _pkg_PrimaryKeyType; }
        }
        #endregion

        #region method
        public static ColumnMapping GetMappingAttribute(object Column)
        {
            return GetMappingAttribute(Column.GetType());
        }

        public static ColumnMapping GetMappingAttribute(PropertyInfo Property)
        {
            ColumnMapping Mapping = null;
            object[] attributes = Property.GetCustomAttributes(typeof(ColumnMapping), false);
            if (attributes != null && attributes.Length > 0)
            {
                Mapping = attributes[0] as ColumnMapping;
            }
            return Mapping;
        }

        public static ColumnMapping GetMappingAttribute(Type Type)
        {
            ColumnMapping Mapping = null;
            object[] attributes = Type.GetCustomAttributes(typeof(ColumnMapping), false);
            if (attributes != null && attributes.Length > 0)
            {
                Mapping = attributes[0] as ColumnMapping;
            }
            return Mapping;
        }
        #endregion
    }
}
