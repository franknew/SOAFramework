using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace SOAFramework.Library
{
    public class SoapHelper
    {
        public static string Serialize(object o)
        {
            
            IFormatter formatter = new SoapFormatter();
            string xml = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, o);
                    xml = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (Exception ex)
            {

            }
            return xml;
        }

        public static object Deserialize(string xml)
        {
            IFormatter formatter = new SoapFormatter();
            object o = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    o = formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {

            }
            return o;
        }

        public static T Deserialize<T>(string xml)
        {
            return (T)Deserialize(xml);
        }
    }
}
