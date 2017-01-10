using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library;

namespace UnitTest.Library
{
    [TestClass]
    public class XmlTesting
    {
        private string xml;
        private string testxml;
        [TestInitialize]
        public void Inint()
        {
            xml = File.ReadAllText("D:\\xml.txt");
            TestSoapSerial();

        }

        [TestMethod]
        public void TestSoapSerial()
        {
            TestClassSoap t = new TestClassSoap
            {
                Name = "t",
                Remark = "test",
                Soap = new TestClassSoap
                {
                    Name = "i",
                    Remark = "inner"
                },
            };
            testxml = SoapHelper.Serialize(t);
        }

        [TestMethod]
        public void TestSoapDeserial()
        {
            var s = SoapHelper.Deserialize(testxml);

        }
    }

    [Serializable]
    public class TestClassSoap
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public TestClassSoap Soap { get; set; }
    }
}
