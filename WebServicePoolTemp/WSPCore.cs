using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;

using SOAFramework.Common;
using Newtonsoft.Json;

namespace SOAFramework.WebServiceCore
{
    /// <summary>
    /// 根据客户端传上来的xml通过反射调用相应的方法
    /// </summary>
    public class WSPCore
    {
        //private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(WSPCore));
        private static string mStr_PreNameSpace = "";
        private static string mStr_IsDebug = ConfigurationSettings.AppSettings["Debug"];
        private static string mStr_AppPath = ConfigurationSettings.AppSettings["AppPath"];
        private static string mStr_ControllerName = ConfigurationSettings.AppSettings["ControllerNameSpace"];
        private static string mStr_ControllerDllPath = "";
        public string CallMethod(string RequestString)
        {
            ClientRequest crRequest = null;
            ServerResponse srResponse = null;
            string strDecodedRequest = null;
            string strResponse = "";
            if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["ControllerName"]))
            {
                mStr_ControllerName = ConfigurationSettings.AppSettings["ControllerName"];
            }
            try
            {
                try
                {
                    strDecodedRequest = System.Web.HttpUtility.UrlDecode(RequestString);
                    crRequest = ClientRequest.GetRequest(strDecodedRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception("解析XML错误，请检查XML格式是否正确！");
                }
                try
                {
                    //通过XML解析出类，方法和参数
                    string strControllerClass = mStr_ControllerName + "." + crRequest.Controller;
                    string strControllerClassType = mStr_ControllerName.Substring(mStr_ControllerName.LastIndexOf(".") + 1, (mStr_ControllerName.Length - mStr_ControllerName.LastIndexOf(".") - 1));
                    string strMethodName = crRequest.MethodName;
                    object[] objArgs = null;
                    if (crRequest.RequestData.MethodArgs != null && crRequest.RequestData.MethodArgs.Length > 0)
                    {
                        objArgs = new object[crRequest.RequestData.MethodArgs.Length];
                        for (int i = 0; i < crRequest.RequestData.MethodArgs.Length; i++)
                        {
                            objArgs[i] = crRequest.RequestData.MethodArgs[0].ArgValue;
                        }
                    }
                    object objResponse = Reflection.CallMethod(MethodName: strMethodName, Args: objArgs, AssemblyName: strControllerClassType, ClassTypeName: strControllerClass);
                    if (objResponse is ServerResponse)
                    {
                        srResponse = objResponse as ServerResponse;
                    }
                    else
                    {
                        srResponse = new ServerResponse();
                        srResponse.ResponseData = objResponse;
                    }
                    srResponse.ResponseType = crRequest.ResponseType;
                    strResponse = srResponse.ToResponseString();
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message;
                }
                //获得参数
                return strResponse;
            }
            catch (Exception ex)
            {
                if (mStr_IsDebug != null && (mStr_IsDebug.ToLower() == "true" || mStr_IsDebug == "1"))
                {
                    Exception exInner = ex;
                    while (exInner != null)
                    {
                        strResponse += "；" + exInner.Message;
                        exInner = exInner.InnerException;
                    }
                }
                else
                {
                    strResponse = "发生错误！";
                }
                //log.Error("Trace:" + ex.StackTrace + "     Message:" + ex.Message + "        Response:" + strResponse + "     XML:" + XML);
                return strResponse;
            }
            finally
            {
                //讲返回值写到前端
                //return strResponse;
                //HttpContext.Current.Response.Write(strResponse);
            }
        }

        public static string GetControllerPath(string AppPath, string ControllerName)
        {
            string strDllPath = "";
            string strAppPath = AppDomain.CurrentDomain.BaseDirectory;
            string strControllerName = "Controller.dll";
            if (!string.IsNullOrEmpty(AppPath))
            {
                strAppPath = AppPath;
            }
            if (!string.IsNullOrEmpty(ControllerName))
            {
                strControllerName = ControllerName;
            }
            strDllPath = (strAppPath.EndsWith("\\") ? strAppPath + strControllerName : strAppPath + "\\" + strControllerName);
            return strDllPath;
        }

        #region 生成调用XML
        /// <summary>
        /// 生成调用服务器web service的xml
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string GenerateCallMethodXML(ClientRequest Request)
        {
            return XMLUtil.XmlSerialize<ClientRequest>(Request);
        }
        /// <summary>
        /// 根据服务器返回的xml,转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static T GetResponseFromXML<T>(string XML)
        {
            return XMLUtil.XmlDeserialize<T>(XML);
        }
        /// <summary>
        /// 根据服务器返回的json,转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T GetResponseFromJson<T>(string Json)
        {
            return JsonConvert.DeserializeObject<T>(Json);
        }

        public static string GenerateCallMethodXML(string Action, string[] Value)
        {
            return GenerateCallMethodXML(null, Action, null, Value);
        }

        public static string GenerateCallMethodXML(string Controller, string Action, string[] Value)
        {
            return GenerateCallMethodXML(Controller, Action, null, Value);
        }

        public static string GenerateCallMethodXML(string Controller, string Action, string[] Key, string[] Value)
        {
            StringBuilder sbXML = new StringBuilder();
            if (Action != string.Empty)
            {
                sbXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sbXML.Append("<WebServicePool><Method ");
                if (Controller != null && Controller != string.Empty)
                {
                    sbXML.Append(" controller=\"" + mStr_PreNameSpace + Controller + "\" ");
                }
                sbXML.Append(" action=\"" + Action + "\" ");
                sbXML.Append(">");
                if (Value != null)
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        sbXML.Append("<Args ");
                        if (Key != null && Key[i] != null && Key[i] != string.Empty)
                        {
                            sbXML.Append(" name=\"" + Key[i] + "\"");
                        }
                        sbXML.Append(">");
                        if (Value[i] != null)
                        {
                            sbXML.Append(Value[i]);
                        }
                        sbXML.Append("</Args>");
                    }
                }
                sbXML.Append("</Method><CityCode>SHA</CityCode></WebServicePool>");
            }
            else
            {
                throw new Exception("没有调用方法名称！");
            }
            return sbXML.ToString();
        }
        #endregion
    }
}
