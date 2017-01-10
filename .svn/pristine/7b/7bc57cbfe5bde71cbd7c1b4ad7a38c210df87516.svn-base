using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{ 
    public class LogHelper
    {
        public static void Write(string log)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHH") + ".log";
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Logs\\";
            string fullFileName = path + fileName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(fullFileName, log);
        }
    }
}
