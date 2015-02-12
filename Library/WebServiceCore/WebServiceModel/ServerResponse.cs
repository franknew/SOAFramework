using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SOAFramework.Library;

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
        private dynamic mObj_ResponseData = null;
        private WSDataType mEnum_ResponseType = WSDataType.MSSchemaXML;
        #endregion

        #region properties
        /// <summary>
        /// 当前页，从1开始计数，用于分页
        /// </summary>
        public int CurrentPageIndex
        {
            set { mInt_CurrentPageIndex = value; }
            get { return mInt_CurrentPageIndex; }
        }
        /// <summary>
        /// 总记录数，用于分页
        /// </summary>
        public int RecordCount
        {
            set { mInt_RecordCount = value; }
            get { return mInt_RecordCount; }
        }
        /// <summary>
        /// 总页数，用于分页
        /// </summary>
        public int PageCount
        {
            set { mInt_PageCount = value; }
            get { return mInt_PageCount; }
        }
        /// <summary>
        /// 如果服务器报错了，这里会有错误信息
        /// </summary>
        public string ErrorMessage
        {
            set { mStr_ErrorMessage = value; }
            get { return mStr_ErrorMessage; }
        }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public dynamic ResponseData
        {
            set { mObj_ResponseData = value; }
            get { return mObj_ResponseData; }
        }
        /// <summary>
        /// 返回数据的格式
        /// </summary>
        public WSDataType ResponseType
        {
            set { mEnum_ResponseType = value; }
            get { return mEnum_ResponseType; }
        }
        /// <summary>
        /// 是否调用成功，true/false
        /// </summary>
        public bool CallSucceded
        {
            set { mBl_CallSucceded = value; }
            get { return mBl_CallSucceded; }
        }

        /// <summary>
        /// 堆栈
        /// </summary>
        public string StackTrace { get; set; }
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
            return XMLHelper.Serialize<ServerResponse>(this);
        }
        /// <summary>
        /// 返回JSON格式的string
        /// </summary>
        /// <returns></returns>
        public string ReturnJSON()
        {
            return JsonHelper.Serialize(this);
        }
        /// <summary>
        /// 从XML加载
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        private static ServerResponse LoadFromXML(string XML)
        {
            return XMLHelper.Deserialize<ServerResponse>(XML);
        }

        /// <summary>
        /// 从JSON加载
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        private static ServerResponse LoadFromJSON(string JSON)
        {
            return JsonHelper.Deserialize<ServerResponse>(JSON);
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
