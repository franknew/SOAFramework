using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SOAFramework.ORM.Mapping
{
    /// <summary>
    /// 表的映射关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableMapping : System.Attribute
    {
        #region attributes
        private string mStr_DataBaseName = "";
        private string mStr_DBOwner = "";
        private string mStr_TableName = "";
        private string mStr_FullTableName = "";
        private SchemaType mEnmu_StructType = SchemaType.Table;
        private Dictionary<string, ColumnMapping> mDic_ColumnMapping = new Dictionary<string, ColumnMapping>();
        #endregion

        #region properties
        public string DataBaseName
        {
            get { return mStr_DataBaseName; }
            set 
            { 
                mStr_DataBaseName = value;
                mStr_FullTableName = LinkIntoFullTableName(mStr_DataBaseName, mStr_DBOwner, mStr_TableName);
            }
        }

        public string DBOwner
        {
            get { return mStr_DBOwner; }
            set 
            {
                mStr_DBOwner = value;
                mStr_FullTableName = LinkIntoFullTableName(mStr_DataBaseName, mStr_DBOwner, mStr_TableName);
            }
        }

        public string TableName
        {
            get { return mStr_TableName; }
            set 
            {
                mStr_TableName = value;
                mStr_FullTableName = LinkIntoFullTableName(mStr_DataBaseName, mStr_DBOwner, mStr_TableName);
            }
        }

        public string FullTableName
        {
            get { return mStr_FullTableName; }
        }

        public SchemaType SchemaType
        {
            get { return mEnmu_StructType; }
            set { mEnmu_StructType = value; }
        }

        public Dictionary<string, ColumnMapping> ColumnsMapping
        {
            get { return mDic_ColumnMapping; }
            set { mDic_ColumnMapping = value; }
        }
        #endregion

        #region contructor
        public static TableMapping CreateInstance(Type Type)
        {
            TableMapping Mapping = new TableMapping();
            Mapping = TableMapping.GetMappingAttribute(Type);
            PropertyInfo[] piArray = Type.GetProperties();
            foreach (PropertyInfo piTemp in piArray)
            {
                ColumnMapping CMTemp = ColumnMapping.GetMappingAttribute(piTemp);
                if (CMTemp != null)
                {
                    Mapping.ColumnsMapping.Add(piTemp.Name, CMTemp);
                }
            }
            return Mapping;
        }
        #endregion

        #region private method
        private string LinkIntoFullTableName(string DatabaseName, string DBOwner, string TableName)
        {
            StringBuilder sbFullTableName = new StringBuilder();
            sbFullTableName.Append(string.IsNullOrEmpty(DatabaseName) ? string.Empty : "[" + DatabaseName + "].");
            sbFullTableName.Append(string.IsNullOrEmpty(DBOwner) ? string.Empty : "[" + DBOwner + "].");
            sbFullTableName.Append(string.IsNullOrEmpty(TableName) ? string.Empty : "[" + TableName + "]");
            return sbFullTableName.ToString();
        }
        #endregion

        #region public method
        public string GetFullTableName(string DBEx)
        {
            StringBuilder sbFullTableName = new StringBuilder();
            if (string.IsNullOrEmpty(DBEx))
            {
                sbFullTableName.Append(mStr_FullTableName);
            }
            else
            {
                sbFullTableName.Append(DBEx);
                if (!DBEx.EndsWith("."))
                {
                    sbFullTableName.Append(".");
                }
                sbFullTableName.Append(mStr_TableName);
            }
            return sbFullTableName.ToString();
        }

        public static TableMapping GetMappingAttribute(object Table)
        {
            return GetMappingAttribute(Table.GetType());
        }

        public static TableMapping GetMappingAttribute(Type Type)
        {
            TableMapping mapping = null;
            object[] attributes = Type.GetCustomAttributes(typeof(TableMapping), false);
            if (attributes != null && attributes.Length > 0)
            {
                mapping = attributes[0] as TableMapping;
            }
            return mapping;
        }
        #endregion
    }
}
