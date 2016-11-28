using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public class XMLPostDataFomatter : IPostDataFormatter
    {
        public string Format(object o)
        {
            string json = JsonHelper.Serialize(o);
            StringBuilder xml = new StringBuilder();
            xml.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            xml.Append("<s:Header>");
            xml.Append("<Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">http://tempuri.org/IService1/Register</Action>");
            xml.Append("</s:Header>");
            xml.Append("<s:Body>");
            xml.Append("<Execute xmlns=\"http://tempuri.org/\">");
            xml.AppendFormat("<args>{0}</args>", json);
            xml.Append("</Execute>");
            xml.Append("</s:Body>");
            xml.Append("</s:Envelope>");
            return xml.ToString();
        }
    }

    public class PostXmlObject
    {
        public object args { get; set; }
    }
}
