using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class JsonSerializerFactory
    {
        public static IJsonSerializer CreateSerializer(JsonSerializerType type)
        {
            IJsonSerializer ser = null;
            switch (type)
            {
                case JsonSerializerType.Microsoft:
                    ser = new MicrosoftSerializer();
                    break;
                case JsonSerializerType.Newtonsoft:
                    ser = new NewtonsoftSerializer();
                    break;
                default:
                    ser = new MicrosoftSerializer();
                    break;
            }
            return ser;
        }
    }

    public enum JsonSerializerType
    {
        Microsoft = 1,
        Newtonsoft = 2,
    }
}
