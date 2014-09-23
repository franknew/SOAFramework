using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frank.Common.DAL;

namespace SOAFramework.ORM.Common
{
    public class Transaction
    {
        #region attribute
        private DBHelper _iHelper = null;
        #endregion

        #region property
        public DBHelper DBHelper
        {
            get { return _iHelper; }
        }
        #endregion

        #region Contructor
        public Transaction(string ConnectionString, string DBType)
        {
            _iHelper = DBFactory.CreateDBHelper(ConnectionString, DBType);
        }

        public Transaction(string ConnectionString)
        {
            _iHelper = DBFactory.CreateDBHelper(ConnectionString);
        }

        public Transaction()
        {
            _iHelper = DBFactory.CreateDBHelper();
        }

        public Transaction(string ConnectionString, DBType DBType)
        {
            _iHelper = DBFactory.CreateDBHelper(ConnectionString, DBType);
        }

        public Transaction(DBType DBType)
        {
            _iHelper = DBFactory.CreateDBHelper(DBType);
        }
        #endregion

        #region transaction controll method
        public void Begin()
        {
            _iHelper.BeginTransaction();
        }

        public void Commit()
        {
            _iHelper.Commit();
        }

        public void RollBack()
        {
            _iHelper.RollBack();
        }
        #endregion
    }
}
