using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public interface IPostDataFormatter
    {
        string Format(IDictionary<string, object> o);
    }

    public class PostDataFormatterFactory
    {
        public static IPostDataFormatter Create(ContentTypeEnum type)
        {
            switch (type)
            {
                case ContentTypeEnum.Xml:
                    return new XMLPostDataFomatter();
                case ContentTypeEnum.UrlEncoded:
                    return new FormPostDataFormatter();
                default:
                    return new JsonPostDataFormatter();
            }
        }
    }
}
