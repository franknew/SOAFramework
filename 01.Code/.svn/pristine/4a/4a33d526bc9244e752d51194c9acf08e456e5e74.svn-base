using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.Xml;
using System.IO;

namespace SOAFramework.Library
{
    public class WebServiceCaller
    {
        #region attribute
        private string _str_WSUrl;
        private string _str_Action;
        private Hashtable _ht_Args;
        private Hashtable _ht_SoapHeader;
        private string _str_NameSpace = @"http://tempuri.org/";
        private string _str_FullURL;
        private string _str_FullAction;
        private string _str_FullResponseString;
        private string _str_ResponseSring;
        #endregion

        #region property
        public string FullResponseString
        {
            get { return _str_FullResponseString; }
        }

        public string ResponseString
        {
            get { return _str_ResponseSring; }
        }

        public string WSUrl
        {
            get { return _str_WSUrl; }
            set 
            {
                _str_WSUrl = value;
                _str_FullURL = GetFullURL(value, _str_Action);
            }
        }

        public string Action
        {
            get { return _str_Action; }
            set 
            { 
                _str_Action = value;
                _str_FullURL = GetFullURL(_str_WSUrl, value);
                _str_FullAction = GetFullURL(_str_NameSpace, value);
            }
        }

        public string NameSpace
        {
            get { return _str_NameSpace; }
            set 
            {
                _str_NameSpace = value;
                _str_FullAction = GetFullURL(value, _str_Action);
            }
        }

        public Hashtable Args
        {
            get { return _ht_Args; }
            set { _ht_Args = value; }
        }

        public Hashtable SopaHeader
        {
            get { return _ht_SoapHeader; }
            set { _ht_SoapHeader = value; }
        }
        #endregion

        #region private helper method
        private string GetFullURL(string WSUrl, string Action)
        {
            string strReturn = "";
            if (!string.IsNullOrEmpty(WSUrl) && !WSUrl.EndsWith("/"))
            {
                WSUrl += "/";
            }
            strReturn = WSUrl + Action;
            return strReturn;
        }

        private string GetSoapString(SoapHeader header)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sbSoap = new StringBuilder();
            sbSoap.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sbSoap.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
            sbSoap.Append(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"> ");
            if (header != null)
            {

            }

            sbSoap.Append("<soap:Body>");
            sbSoap.Append("<").Append(_str_Action).Append(" xmlns=\"").Append(_str_NameSpace).Append("\"").Append(">");
            if (null != _ht_Args && 0 < _ht_Args.Count)
            {
                foreach (string strKey in _ht_Args.Keys)
                {
                    sbSoap.Append("<").Append(strKey).Append(">");
                    sbSoap.Append(_ht_Args[strKey]);
                    sbSoap.Append("</").Append(strKey).Append(">");
                }
            }
            sbSoap.Append("</").Append(_str_Action).Append(">");
            sbSoap.Append("</soap:Body>");
            sbSoap.Append("</soap:Envelope>");
            return sbSoap.ToString();
        }

        #region public method
        public string SoapCall()
        {
            string strResult = "";
            string strPostString = GetSoapString(null);
            byte[] bytPostString = Encoding.UTF8.GetBytes(strPostString);
            HttpWebRequest webRequest = WebRequest.Create(_str_WSUrl) as HttpWebRequest;
            webRequest.Headers.Add(@"SOAPAction", _str_FullAction);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContentLength = bytPostString.Length;
            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(bytPostString, 0, bytPostString.Length);
            dataStream.Close();
            WebResponse webResponse = webRequest.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader srResponse = new StreamReader(responseStream);
            _str_FullResponseString = srResponse.ReadToEnd();
            srResponse.Close();
            XmlDocument xdResponse = new XmlDocument();
            xdResponse.LoadXml(_str_FullResponseString);
            XmlNodeList xnlResponse = xdResponse.GetElementsByTagName(_str_Action + "Result");
            if (null != xnlResponse && 0 < xnlResponse.Count)
            {
                _str_ResponseSring = xnlResponse[0].InnerText;
                strResult = _str_ResponseSring;
            }
            return strResult;
        }
        #endregion
        #endregion
    }
}
