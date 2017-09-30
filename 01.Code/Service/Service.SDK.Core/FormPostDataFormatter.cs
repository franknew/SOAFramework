﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SOAFramework.Service.SDK.Core
{
    public class FormPostDataFormatter : IPostDataFormatter
    {
        public string Format(IDictionary<string, object> o)
        {
            if (o == null) return null;
            StringBuilder builder = new StringBuilder();

            foreach (var key in o.Keys)
            {
                object value = o[key];
                var valueType = value.GetType();
                if (valueType.IsClass && !valueType.Equals(typeof(string))) builder.Append(value.ToString()).Append("&");
                else builder.AppendFormat("{0}=", key).Append(HttpUtility.UrlEncode(value.ToString())).Append("&");
            }
            
            string result = builder.ToString().TrimEnd('&');
            return result;
        }
    }
}
