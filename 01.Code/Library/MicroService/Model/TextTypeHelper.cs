using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MicroService.Library;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class TextTypeTypeHelper
    {
        public static TextType Check(string text)
        {
            text = HttpUtility.UrlDecode(text);
            TextType type = TextType.PlainText;
            if ((text.StartsWith("<?") || text.StartsWith("<")) && (text.EndsWith("/>") || text.EndsWith(">"))) type = TextType.Xml;
            else if ((text.StartsWith("{") && text.EndsWith("}")) || (text.StartsWith("[") && text.EndsWith("]"))) type = TextType.Json;
            return type;
        }

        public static ContentTypeEnum ToSerialType(TextType type)
        {
            ContentTypeEnum serailType = ContentTypeEnum.Nothing;
            switch (type)
            {
                case TextType.Json:
                    serailType = ContentTypeEnum.Json;
                    break;
                case TextType.PlainText:
                    break;
                case TextType.Xml:
                    serailType = ContentTypeEnum.Xml;
                    break;
            }
            return serailType;
        }
    }

    public enum TextType
    {
        PlainText,
        Json,
        Xml,
    }
}
