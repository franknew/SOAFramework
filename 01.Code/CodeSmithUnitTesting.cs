using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CodeSmith.Engine;

namespace UnitTest
{
    /// <summary>
    /// CodeSmithUnitTesting 的摘要说明
    /// </summary>
    [TestClass]
    public class CodeSmithUnitTesting
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory + @"Templates\Smiple.cst";

        private string content = "<%@ Template Language=\"C#\" TargetLanguage=\"Text\" %>\r\n<%@ Property Name=\"SampleStringProperty\" Default=\"SomeValue\" Type=\"System.String\" %>" +
            "hello world <%=SampleStringProperty%>";

        public CodeSmithUnitTesting()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
         //在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
         [ClassCleanup()]
         public static void MyClassCleanup()
        {
            FileInfo template = new FileInfo(path);
             if (template.Exists)
             {
                 template.Delete();
             }
         }
        //
         //在运行每个测试之前，使用 TestInitialize 来运行代码
         [TestInitialize()]
         public void MyTestInitialize() 
         {
             FileInfo template = new FileInfo(path);
             if (!template.Exists)
             {
                 if (!template.Directory.Exists)
                 {
                     template.Directory.Create();
                 }
                 File.AppendAllText(path, content, Encoding.UTF8);
             }

         }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RenderTesting()
         {
             TemplateEngine engine = new TemplateEngine(System.IO.Path.GetDirectoryName(path));
             CompileTemplateResult result = engine.Compile(path);
             if (!result.Errors.HasErrors)
             {
                 CodeTemplate template = result.CreateTemplateInstance();
                 template.SetProperty("SampleStringProperty", "frank");
                 string render = template.RenderToString();
                 Assert.AreEqual("hello world frank", render);
             }
        }
    }
}
