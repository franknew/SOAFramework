using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Web;

using SOAFramework.Library;

namespace SOAFramework.WebServiceCore
{
    [Serializable]
    public class ClientRequest
    {
        #region attributes
        private string mStr_Controller;
        private string mStr_MethodName;
        private ClientInfo mObj_ClinetInfo;
        private RequestData mObj_RequestData;
        private string mStr_AuthKey = "";
        private string mStr_WebServiceURL = "";
        private string mStr_CallMethodName = "Call";
        private WSDataType mEnum_RequestType = WSDataType.MSSchemaXML;
        private WSDataType mEnum_ResponseType = WSDataType.MSSchemaXML;
        #endregion

        #region properties
        public void GetClientInfoFromMachine()
        {
            mObj_ClinetInfo = new ClientInfo();
        }
        public ClientInfo ClientInfo
        {
            set { mObj_ClinetInfo = value; }
            get { return mObj_ClinetInfo; }
        }
        public RequestData RequestData
        {
            set { mObj_RequestData = value; }
            get { return mObj_RequestData; }
        }
        public string AuthKey
        {
            set { mStr_AuthKey = value; }
            get { return mStr_AuthKey; }
        }
        public WSDataType ResponseType
        {
            set { mEnum_ResponseType = value; }
            get { return mEnum_ResponseType; }
        }
        public WSDataType RequestType
        {
            set { mEnum_RequestType = value; }
            get { return mEnum_RequestType; }
        }
        public string Controller
        {
            set { mStr_Controller = value; }
            get { return mStr_Controller; }
        }
        public string MethodName
        {
            set { mStr_MethodName = value; }
            get { return mStr_MethodName; }
        }
        #endregion

        #region methods
        public ClientRequest(string AuthKey)
        {
            mStr_AuthKey = AuthKey;
        }
        public ClientRequest()
        {
            mObj_RequestData = new RequestData();
            mObj_ClinetInfo = new ClientInfo();
            string strWebServiceURL = ConfigurationSettings.AppSettings["WebServiceURL"];
            string strCallMethodName = ConfigurationSettings.AppSettings["CallMethodName"];
            if (!string.IsNullOrEmpty(strWebServiceURL))
            {
                mStr_WebServiceURL = strWebServiceURL;
            }
            if (!string.IsNullOrEmpty(strCallMethodName))
            {
                mStr_CallMethodName = strCallMethodName;
            }
        }

        private string ToXML()
        {
            return XMLHelper.Serialize<ClientRequest>(this);
        }

        private string ToJSON()
        {
            return JsonHelper.Serialize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetRequestString()
        {
            string strRequest;
            string strRequestHeader;
            switch (RequestType)
            {
                case WSDataType.JSON://JSON
                    strRequest = this.ToJSON();
                    strRequestHeader = "j";
                    break;
                case WSDataType.CustomSchemaXML://Custom Schema XML
                    strRequest = this.ToXML();
                    strRequestHeader = "x";
                    break;
                default://MS XML
                    strRequest = this.ToXML();
                    strRequestHeader = "x";
                    break;
            }
            strRequest = strRequestHeader + strRequest;
            return strRequest;
        }
        /// <summary>
        /// 发送请求到服务器端
        /// </summary>
        /// <param name="Service">web service对象</param>
        /// <param name="ActionName">调用的web service方法</param>
        /// <returns></returns>
        public string SendRequest(object Service, string ActionName)
        {
            string strResponse = "";
            MethodInfo miService = Service.GetType().GetMethod(ActionName);
            if (miService != null)
            {
                string strRequestString = GetRequestString();
                strResponse = miService.Invoke(Service, new string[] { strRequestString }).ToString();
            }
            return strResponse;
        }
        /// <summary>
        /// 发送请求到服务器端
        /// </summary>
        /// <param name="URL">web service的.asmx文件链接</param>
        /// <param name="MethodName">调用的web service方法</param>
        /// <returns></returns>
        public string SendRequest(string URL, string MethodName)
        {
            //string strRequestString = "RequestString=" + GetRequestString();
            //return HttpUtil.VisitWebPage_Post(URL, MethodName, strRequestString);
            Hashtable htArgs = new Hashtable();
            string strRequestString = GetRequestString();
            htArgs["RequestString"] = strRequestString;
            WebServiceCaller wsCaller = new WebServiceCaller();
            wsCaller.Action = MethodName;
            wsCaller.Args = htArgs;
            wsCaller.WSUrl = URL;
            return wsCaller.SoapCall();
        }

        public string SendRequest(string MethodName)
        {
            return SendRequest(mStr_WebServiceURL, MethodName);
        }

        public string SendRequest()
        {
            return SendRequest(mStr_WebServiceURL, mStr_CallMethodName);
        }

        public ServerResponse GetResponse(string URL, string MethodName)
        {
            string strResponseString = this.SendRequest(URL, MethodName);
            ServerResponse srResponse = ServerResponse.LoadFromResponse(strResponseString);
            return srResponse;
        }

        public ServerResponse GetResponse(string MethodName)
        {
            return GetResponse(mStr_WebServiceURL, MethodName);
        }

        public ServerResponse GetResponse()
        {
            return GetResponse(mStr_WebServiceURL, mStr_CallMethodName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestString"></param>
        /// <returns></returns>
        public static ClientRequest GetRequest(string RequestString)
        {
            ClientRequest crRequest = null;
            string strRequestType = RequestString.Substring(0, 1);
            string strRequest = RequestString.Remove(0, 1);
            switch (strRequestType.ToLower())
            {
                case "j":
                    crRequest = GetRequestFromJSON(strRequest);
                    break;
                default:
                    crRequest = GetRequestFromXML(strRequest);
                    break;
            }
            return crRequest;
        }

        public static ClientRequest GetRequestFromXML(string XML)
        {
            ClientRequest crRequest = XMLHelper.Deserialize<ClientRequest>(XML);
            return crRequest;
        }

        public static ClientRequest GetRequestFromJSON(string JSON)
        {
            ClientRequest crRequest = JsonHelper.Deserialize<ClientRequest>(JSON);
            return crRequest;
        }
        #endregion

    }

    public class ClientInfo
    {
        #region attributes
        private string mStr_IP4;
        private string mStr_IP6;
        private string mStr_MachineName;
        private string mStr_Domain;
        private string mStr_AppName;
        private string mStr_MacAddress;
        private string mStr_OSName;
        private string mStr_OSVersion;
        private string mStr_FunctionName;
        #endregion

        #region properties
        public string FunctionName
        {
            set { mStr_FunctionName = value; }
            get { return mStr_FunctionName; }
        }
        public string IP4
        {
            set { mStr_IP4 = value; }
            get { return mStr_IP4; }
        }
        public string IP6
        {
            set { mStr_IP6 = value; }
            get { return mStr_IP6; }
        }
        public string MachineName
        {
            set { mStr_MachineName = value; }
            get { return mStr_MachineName; }
        }
        public string AppName
        {
            set { mStr_AppName = value; }
            get { return mStr_AppName; }
        }
        public string Domain
        {
            set { mStr_Domain = value; }
            get { return mStr_Domain; }
        }
        public string MacAddress
        {
            set { mStr_MacAddress = value; }
            get { return mStr_MacAddress; }
        }
        public string OSName
        {
            set { mStr_OSName = value; }
            get { return mStr_OSName; }
        }
        public string OSVersion
        {
            set { mStr_OSVersion = value; }
            get { return mStr_OSVersion; }
        }
        #endregion

        #region methods
        public ClientInfo()
        {
            StackTrace stTemp = new StackTrace();
            this.FunctionName = stTemp.GetFrame(2).GetMethod().Name;
        }
        public static ClientInfo GetInfoFromClient()
        {
            ClientInfo ciSelf = new ClientInfo();
            StackTrace stTemp = new StackTrace();
            ciSelf.FunctionName = stTemp.GetFrame(1).GetMethod().Name;
            return null;
        }
        #endregion
    }

    public class RequestData
    {
        #region attributes
        private MethodArg[] mArr_MethodArg;
        #endregion

        #region properties
        /// <summary>
        /// 方法的入参，必须和服务器端的方法顺序相对应
        /// </summary>
        public MethodArg[] MethodArgs
        {
            set { mArr_MethodArg = value; }
            get { return mArr_MethodArg; }
        }
        #endregion
    }

    public class MethodArg
    {
        #region attributes
        private string mStr_ArgName;
        private object mObj_ArgValue;
        private string mTyp_ArgType;
        private int mInt_Length;
        #endregion

        #region properties
        public string ArgName
        {
            set { mStr_ArgName = value; }
            get { return mStr_ArgName; }
        }
        public object ArgValue
        {
            set { mObj_ArgValue = value; mTyp_ArgType = value.GetType().FullName; }
            get { return mObj_ArgValue; }
        }
        public string ArgType
        {
            get { return mTyp_ArgType; }
        }
        public int Length
        {
            set { mInt_Length = value; }
            get { return mInt_Length; }
        }
        #endregion

        #region methods
        public MethodArg(object Value)
        {
            this.ArgValue = Value;
        }

        public MethodArg()
        {
        }
        #endregion
    }

    public enum WSDataType
    {
        JSON,
        CustomSchemaXML,
        MSSchemaXML
    }
}
