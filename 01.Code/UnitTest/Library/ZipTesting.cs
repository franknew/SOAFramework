using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAFramework.Library;
using SOAFramework.Library.Lib;

namespace UnitTest.Library
{
    [TestClass]
    public class ZipTesting
    {
        [TestMethod]
        public void ZipTest()
        {
            string orizip = "i am a string, to be zipped!";
            string zipped = ZipHelper.Zip(orizip);
            Assert.AreEqual("H4sIAAAAAAAEAMtUSMxVSFQoLinKzEvXUSjJV0hKVajKLChITVEEANqadAkcAAAA", zipped);
        }

        [TestMethod]
        public void UnzipTest()
        {
            string zipped = "H4sIAAAAAAAEAMtUSMxVSFQoLinKzEvXUSjJV0hKVajKLChITVEEANqadAkcAAAA";
            string unzipped = ZipHelper.UnZip(zipped);
            Assert.AreEqual("i am a string, to be zipped!", unzipped);
        }
    }
}
