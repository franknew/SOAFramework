using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Threading;
using SOAFramework.WebServiceCore;

namespace SOAFramework.WebServicePool
{
    /// <summary>
    /// WebServicePool 的摘要说明
    /// </summary>
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebServicePool : System.Web.Services.WebService
    {
        private delegate string CallMethod(string XML);//用于异步调用的代理
        /// <summary>
        /// 调用controller里面的方法
        /// </summary>
        /// <param name="XML">controller xml</param>
        /// <returns>xml数据</returns>
        [WebMethod]
        public string Call(string RequestString)
        {
            string strResponse = "";
            strResponse = new WSPCore().CallMethod(RequestString);
            return strResponse;
        }

        [WebMethod]
        public string PostTest(string str)
        {
            return str;
        }

        [WebMethod]
        public string GetTest()
        {
            return "ok";
        }
    }
}
