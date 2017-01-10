using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SOAFramework.Library;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace MicroService.Library
{
    public class UrlSerializor : ISerializable
    {
        public object Deserialize(string text, Type t)
        {
            var args = text.Split('&');
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (args.Length == 0) return dic;
            object obj = Activator.CreateInstance(t);
            foreach (var arg in args)
            {
                var keyvalue = arg.Split('=');
                string key = keyvalue[0];
                if (string.IsNullOrEmpty(key)) continue;
                string value = null;
                if (keyvalue.Length > 1) value = keyvalue[1];
                object saveValue = value;
                if (!string.IsNullOrEmpty(value))
                {
                    value = HttpUtility.UrlDecode(value.Trim());
                    var textType = TextTypeTypeHelper.Check(value);
                    var serialType = TextTypeTypeHelper.ToSerialType(textType);
                    var serial = SerializeFactory.Create(serialType);
                    if (serial != null) saveValue = serial.Deserialize<JToken>(value);
                }
                dic[key] = saveValue;

            }
            var type = TypeUtility.CheckType(t);
            foreach (var key in dic.Keys)
            {
                switch (type)
                {
                    case ObjectTypeEnum.Class:
                        obj.TrySetValue(key, dic[key]);
                        break;
                    case ObjectTypeEnum.Dictionary:
                        IDictionary iDic = obj as IDictionary;
                        iDic[key] = dic[key];
                        break;
                }
            }
            return obj;
        }

        public T Deserialize<T>(string text)
        {
            return (T)Deserialize(text, typeof(T));
        }

        public string Serialize(object o)
        {
            return JsonHelper.Serialize(o);
        }
    }
}
