using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SOAFramework.Common;

namespace SOAFramework.WebServiceCore
{
    public class ServerResponse
    {

        #region attributes
        private int mInt_CurrentPageIndex = -1;
        private int mInt_RecordCount = -1;
        private int mInt_PageCount = -1;
        private bool mBl_CallSucceded = true;
        private string mStr_ErrorMessage = "";
        private object mObj_ResponseData = null;
        private WSDataType mEnum_ResponseType = WSDataType.MSSchemaXML;
        #endregion

        #region properties
        public int CurrentPageIndex
        {
            set { mInt_CurrentPageIndex = value; }
            get { return mInt_CurrentPageIndex; }
        }
        public int RecordCount
        {
            set { mInt_RecordCount = value; }
            get { return mInt_RecordCount; }
        }
        public int PageCount
        {
            set { mInt_PageCount = value; }
            get { return mInt_PageCount; }
        }
        public string ErrorMessage
        {
            set { mStr_ErrorMessage = value; }
            get { return mStr_ErrorMessage; }
        }
        public object ResponseData
        {
            set { mObj_ResponseData = value; }
            get { return mObj_ResponseData; }
        }
        public WSDataType ResponseType
        {
            set { mEnum_ResponseType = value; }
            get { return mEnum_ResponseType; }
        }
        public bool CallSucceded
        {
            set { mBl_CallSucceded = value; }
            get { return mBl_CallSucceded; }
        }
        #endregion

        #region methods
        /// <summary>
        /// 生成用来返回的string
        /// </summary>
        /// <returns></returns>
        public string ToResponseString()
        {
            string strReturn = "";
            switch (this.mEnum_ResponseType)
            {
                case WSDataType.JSON:
                    strReturn = "j" + ReturnJSON();
                    break;
                default:
                    strReturn = "x" + ReturnMSXML();
                    break;
            }
            return strReturn;
        }
        /// <summary>
        /// 返回MS默认的XML格式的string
        /// </summary>
        /// <returns></returns>
        public string ReturnMSXML()
        {
            return XMLUtil.XmlSerialize<ServerResponse>(this);
        }
        /// <summary>
        /// 返回JSON格式的string
        /// </summary>
        /// <returns></returns>
        public string ReturnJSON()
        {
            return JSONUtil.JSONSerialize(this);
        }
        /// <summary>
        /// 从XML加载
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        private static ServerResponse LoadFromXML(string XML)
        {
            return XMLUtil.XmlDeserialize<ServerResponse>(XML);
        }
        /// <summary>
        /// 从JSON加载
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        private static ServerResponse LoadFromJSON(string JSON)
        {
            return JSONUtil.JSONDeserialize<ServerResponse>(JSON);
        }
        /// <summary>
        /// 从服务器返回的string,加载成ServerResponse对象
        /// </summary>
        /// <param name="Response"></param>
        /// <returns></returns>
        public static ServerResponse LoadFromResponse(string Response)
        {
            string strResponseType = Response.Substring(0, 1);
            string strResponseData = Response.Remove(0, 1);
            ServerResponse objResponse = null;
            switch (strResponseType.ToLower())
            {
                case "j"://JSON
                    objResponse = LoadFromJSON(strResponseData);
                    objResponse.mEnum_ResponseType = WSDataType.JSON;
                    break;
                default://default XML
                    objResponse = LoadFromXML(strResponseData);
                    objResponse.mEnum_ResponseType = WSDataType.MSSchemaXML;
                    break;
            }
            return objResponse;
        }
        #endregion

    }
}
