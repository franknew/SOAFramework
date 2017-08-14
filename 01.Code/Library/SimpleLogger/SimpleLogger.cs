using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class SimpleLogger
    {
        private string _logpath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        private string _fileNameFormat = "yyyyMMdd";
        private static object locker = new object();

        public SimpleLogger(string logpath = "", string fileNameFormat = "")
        {
            if (!string.IsNullOrEmpty(logpath)) _logpath = logpath;
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
            lock (locker)
            {
                File.AppendAllLines(file.FullName, new List<string> { log.ToString() });
            }
        }

        public void WriteException(Exception ex, bool hasTimeStamp = true)
        {
            StringBuilder message = new StringBuilder();
            StringBuilder traces = new StringBuilder();
            StringBuilder source = new StringBuilder();
            StringBuilder err = new StringBuilder();
            while (ex != null)
            {
                message.Append(ex.Message);
                traces.Append(ex.StackTrace);
                source.Append(ex.Source);
                ex = ex.InnerException;
            }
            err.AppendFormat("Message:{0}{3}Stack Trace:{1}{3}Source:{2}{3}", message, traces, source, Environment.NewLine);
            this.Write(err.ToString(), hasTimeStamp);
        }
    }
}
