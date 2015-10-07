using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using SOAFramework.Common;

namespace SOAFramework.WebServiceCore
{
    /// <summary>
    /// 解析客户端传来的xml实体类
    /// </summary>
    public class Controller
    {
        private string _strControllerName = "";
        private string _strActionName = "";
        private string[] _strArgName = null;
        private string[] _strArgValue = null;
        private string[][] _arrArgMap = null;
        private string _strDllFileName = "App_Code.dll";
        private static string _strDllFilePath = ConfigurationSettings.AppSettings["DllFilePath"];
        private static string _strTranditionalList = ConfigurationSettings.AppSettings["TranditionalList"];
        //private ConvertType _typLanguage = ConvertType.Simplified;

        public string ControllerName
        {
            get { return _strControllerName; }
        }

        public string ActionName
        {
            get { return _strActionName; }
        }

        public string[] ArgValue
        {
            get { return _strArgValue; }
        }

        public string[] ArgName
        {
            get { return _strArgName; }
        }

        public string DllFileName
        {
            get { return _strDllFileName; }
        }

        public string[][] ArgMap
        {
            get { return _arrArgMap; }
        }

        //public ConvertType LanguageType
        //{
        //    get { return _typLanguage; }
        //}

        //public static Controller InitController(string XML)
        //{
        //    return ReadXML(XML);
        //}

        public static Controller InitController(string XML)
        {
            return XMLUtil.XmlDeserialize<Controller>(XML);
        }

        /// <summary>
        /// 读取xml，转换成controller对象
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        private static Controller ReadXML(string XML)
        {
            Controller crReturn = new Controller();
            XmlDocument xdMain = new XmlDocument();
            xdMain.LoadXml(XML);
            //获得调用方法的节点
            XmlNode xnWebServicePool = xdMain.SelectSingleNode("WebServicePool");
            XmlNode xnCityCode = null;
            XmlNode xnMethod = null;
            if (xnWebServicePool != null)
            {
                xnMethod = xnWebServicePool.SelectSingleNode("Method");
                xnCityCode = xnWebServicePool.SelectSingleNode("CityCode");
            }
            else
            {
                xnMethod = xdMain.SelectSingleNode("Method");
            }
            XmlNodeList xnlArgs = xnMethod.ChildNodes;
            //获得多国语言设置的节点
            if (xnCityCode != null)
            {
                //是否属于繁体区域
                if (_strTranditionalList != null && _strTranditionalList.ToUpper().IndexOf(xnCityCode.InnerText.ToUpper()) > -1)
                {
                    //crReturn._typLanguage = ConvertType.Traditional;
                }
            }
            if (xnMethod.Attributes["controller"] != null)
            {
                //调用逻辑类
                crReturn._strControllerName = "Controller." + xnMethod.Attributes["controller"].Value;
                if (_strDllFilePath == null)
                {
                    crReturn._strDllFileName = "Controller.dll";
                }
                else
                {
                    if (_strDllFilePath.EndsWith("\\"))
                    {
                        crReturn._strDllFileName = _strDllFilePath + "Controller.dll";
                    }
                    else
                    {
                        crReturn._strDllFileName = _strDllFilePath + "\\" + "Controller.dll";
                    }
                }
            }
            if (xnMethod.Attributes["action"] != null)
            {
                crReturn._strActionName = xnMethod.Attributes["action"].Value;
            }
            if (xnlArgs != null && xnlArgs.Count > 0)
            {
                crReturn._strArgName = new string[xnlArgs.Count];
                crReturn._strArgValue = new string[xnlArgs.Count];
                for (int i = 0; i < xnlArgs.Count; i++)
                {
                    crReturn._strArgName[i] = "";
                    crReturn._strArgValue[i] = "";
                    if (xnlArgs[i].Attributes["name"] != null)
                    {
                        crReturn._strArgName[i] = xnlArgs[i].Attributes["name"].Value;
                    }
                    //转换成简体传入
                    //crReturn._strArgValue[i] = EncondingConverter.SCTCConvert(ConvertType.Simplified, SQLValueCheck(xnlArgs[i].InnerText));
                    crReturn._strArgValue[i] = SQLValueCheck(xnlArgs[i].InnerText);
                }
            }
            return crReturn;
        }

        /// <summary>
        /// 去掉单引号，参数安全检测
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static string SQLValueCheck(string Value)
        {
            string strReturn = null;
            if (Value != null)
            {
                strReturn = Value.Replace("'", "''").Replace("‘", "''").Replace("’", "''");
            }
            return strReturn;
        }
        
    }
}
