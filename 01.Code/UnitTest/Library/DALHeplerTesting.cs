using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library.DAL;

namespace UnitTest.Library
{
    [TestClass]
    public class DALHeplerTesting
    {
        private IDBHelper helper;
        [ClassInitialize]
        public void init()
        {
            //helper = DBFactory.CreateDBHelper();
        }


        [TestMethod]
        public void insertAndQueryTest()
        {
            helper = DBFactory.CreateDBHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Test ('100','unit test','1')");
            Assert.AreEqual(1, helper.ExecNoneQueryWithSQL(builder.ToString()));
        }
    }
}
