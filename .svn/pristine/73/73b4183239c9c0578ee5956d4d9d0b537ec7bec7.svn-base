using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.SDK.Core
{
    public interface IPostDataFormatter
    {
        string Format(object o);
    }

    public class PostDataFormatterFactory
    {
        public static IPostDataFormatter Create(PostDataFomaterType type)
        {
            switch (type)
            {
                case PostDataFomaterType.XML:
                    return new XMLPostDataFomatter();
                default:
                    return new JsonPostDataFormatter();
            }
        }
    }

    public enum PostDataFomaterType
    {
        Json,
        XML,
    }
}
