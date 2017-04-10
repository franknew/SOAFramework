﻿using System;
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
    }
}
