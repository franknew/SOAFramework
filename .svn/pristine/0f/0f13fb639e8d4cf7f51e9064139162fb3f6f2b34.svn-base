using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library;
using System.Collections.Generic;

namespace UnitTest.Library
{
    [TestClass]
    public class JsonTesting
    {
        private string json = "{\"a\":\"a\",\"b\":\"b\",\"d\":\"d\",\"g\":12,\"test\":[{\"a\":\"a1\",\"g\":0}]}";

        [TestMethod]
        public void ToJsonTest()
        {
            TestClass a = new TestClass();
            a.a = "a";
            a.b = "b";
            a.d = "d";
            a.g = 12;
            a.test = new List<TestClass> { new TestClass { a = "a1" } };

            string s = JsonHelper.Serialize(a);
            Assert.AreEqual(json, s);
        }

        [TestMethod]
        public void ToObjectTest()
        {
            TestClass a = JsonHelper.Deserialize<TestClass>(json);
            Assert.AreEqual("a", a.a);
            Assert.AreEqual("b", a.b);
            Assert.AreEqual("d", a.d);
            Assert.AreEqual(12, a.g);
            Assert.IsNotNull(a.test);
            Assert.IsNull(a.dic);
        }
    }

    public class TestClass
    {
        public string a { get; set; }
        public string b { get; set; }

        public string d { get; set; }

        public DateTime? f { get; set; }

        public int? g { get; set; }

        public List<TestClass> test { get; set; }

        public Dictionary<string, string> dic { get; set; }
    }
}
