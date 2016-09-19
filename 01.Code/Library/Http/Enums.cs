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
    }

    public class ContentTypeConvert
    {
        public static ContentTypeEnum Convert(string contentType)
        {
            ContentTypeEnum type = ContentTypeEnum.Json;
            switch (contentType)
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
            }
            return type;
        } 
    }
}
