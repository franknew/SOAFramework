using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using SOAFramework.ORM.ORMDriver;
using SOAFramework.ORM.Mapping;

namespace Model
{
    [TableMapping(DBOwner="dbo", TableName="Users")]
    public class Users : TableModelBase<Users>
    {
        #region virables
        protected static string _str_DBEx = "";
        public static string[] _arr_PrimaryKey = new string[] { "PK_UserID" };
        private string _str_PK_UserID = null;
        private string _str_Password = null;
        private string _bl_IsValid = null;
        #endregion

        #region methods

        //public static Users GetModel(string PK_UserID)
        //{
        //    return ModelBase<Users>.GetModel1Param(PK_UserID);
        //}

        //public static Users GetModel(string PK_UserID, string DBEx)
        //{
        //    return ModelBase<Users>.GetModel1Param(PK_UserID, DBEx);
        //}

        //public static bool IsExits(string PK_UserID)
        //{
        //    return ModelBase<Users>.IsExits1Param(PK_UserID);
        //}

        //public static bool IsExits(string PK_UserID, string DBEx)
        //{
        //    return ModelBase<Users>.IsExits1Param(PK_UserID, DBEx);
        //}
        #endregion

        #region properties
        [ColumnMapping(IsPrimaryKey = true)]
        public string PK_UserID
        {
            get { return _str_PK_UserID; }
            set { _str_PK_UserID = value; }
        }

        public string Password
        {
            get { return _str_Password; }
            set { _str_Password = value; }
        }

        public string IsValid
        {
            get { return _bl_IsValid; }
            set { _bl_IsValid = value; }
        }
        #endregion
    }

    public class User : Users
    {
        private string _a;
        public string a
        {
            set { _a = value; }
            get { return _a; }
        }
    }
}
