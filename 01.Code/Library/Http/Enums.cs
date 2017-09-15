using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public enum ContentTypeEnum
    {
        Xml,
        Json,
        Soap,
        UrlEncoded,
        Nothing,
    }

    public class ContentTypeConvert
    {
        public static ContentTypeEnum Convert(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return ContentTypeEnum.UrlEncoded;
            }
            string cType = contentType.Split(';')[0];
            ContentTypeEnum type = ContentTypeEnum.Json;
            switch (cType)
            {
                case "text/xml":
                case "application/xml":
                    type = ContentTypeEnum.Xml;
                    break;
                case "application/json":
                case "text/x-json":
                case "text/json":
                    type = ContentTypeEnum.Json;
                    break;
                case "application/soap+xml":
                    type = ContentTypeEnum.Soap;
                    break;
                case "application/x-www-form-urlencoded":
                    type = ContentTypeEnum.UrlEncoded;
                    break;
                default:
                    type = ContentTypeEnum.Nothing;
                    break;
            }
            return type;
        } 

        public static string ToTypeString(ContentTypeEnum type)
        {
            string typeString = "application/x-www-form-urlencoded";
            switch (type)
            {
                case ContentTypeEnum.Json:
                    typeString = "application/json";
                    break;
                case ContentTypeEnum.Xml:
                    typeString = "text/xml";
                    break;
                case ContentTypeEnum.UrlEncoded:
                    typeString = "application/x-www-form-urlencoded";
                    break;
                case ContentTypeEnum.Soap:
                    typeString = "application/soap+xml";
                    break;
            }
            return typeString;
        }
    }

}
