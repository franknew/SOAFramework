using System;
using System.Collections.Generic;
using System.Text;

namespace SOAFramework.ORM.ModelDriver
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttrDef : System.Attribute
    {
        #region attributes
        //�Ƿ�����
        private bool _bl_IsPrimaryKey = false;
        //�ֶ�����
        private Type _typ_AttrType = null;
        //�ֶγ���
        private int _int_Lenght = -1;
        //�Ƿ�������
        private bool _bl_IsIdentity = false;
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

        public bool IsIdentity
        {
            set { _bl_IsIdentity = value; }
            get { return _bl_IsIdentity; }
        }
        #endregion
    }

    public class ClassAttrDef : System.Attribute
    {
        #region attributes
        private string mStr_DataBaseName;
        private string mStr_DBOwner;
        private string mStr_TableName;
        private DBStructType mEnmu_StructType;
        #endregion

        #region properties
        public string DataBaseName
        {
            get { return mStr_DataBaseName; }
        }

        public string DBOwner
        {
            get { return mStr_DBOwner; }
        }

        public string TableName
        {
            get { return mStr_TableName; }
        }

        public DBStructType StructType
        {
            get { return mEnmu_StructType; }
        }
        #endregion
    }

    public enum DBStructType
    {
        Table,
        View
    }
}
