using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class JsonSerializorFactory
    {
        public static IJsonSerializor CreateSerializor(JsonSerializorType type)
        {
            IJsonSerializor ser = null;
            switch (type)
            {
                case JsonSerializorType.Microsoft:
                    ser = new MicrosoftSerializor();
                    break;
                case JsonSerializorType.Newtonsoft:
                    ser = new NewtonsoftSerializor();
                    break;
                default:
                    ser = new MicrosoftSerializor();
                    break;
            }
            return ser;
        }
    }

    public enum JsonSerializorType
    {
        Microsoft = 1,
        Newtonsoft = 2,
    }
}
