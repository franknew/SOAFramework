using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class SimpleLogger
    {
        private string _logpath = "";
        private string _fileNameFormat = "yyyyMMdd";

        public SimpleLogger(string logpath = "", string fileNameFormat = "")
        {
            _logpath = logpath;
            if (!string.IsNullOrEmpty(fileNameFormat)) _fileNameFormat = fileNameFormat;
        }

        public void Write(string content, bool hasTimeStamp = true)
        {
            string fullfilename = _logpath.TrimEnd('\\') + "\\" + DateTime.Now.ToString(_fileNameFormat) + ".log";
            FileInfo file = new FileInfo(fullfilename);
            if (!file.Directory.Exists) file.Directory.Create();
            StringBuilder log = new StringBuilder();
            if (hasTimeStamp) log.AppendFormat("{0} -- {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
            else log.Append(content);
            File.AppendAllLines(file.FullName, new List<string> { log.ToString() });
        }
    }
}
