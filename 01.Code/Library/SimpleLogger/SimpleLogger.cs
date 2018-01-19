using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SOAFramework.Library
{
    public class SimpleLogger
    {
        private string _logpath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        private string _fileNameFormat = "yyyyMMdd";
        private static object locker = new object();
        public static long fileSize = 5 * 1024 * 1024;

        private static int _errIndex = 0;
        private static int _degIndex = 0;
        private Mutex m = new Mutex();

        public SimpleLogger(string logpath = "", string fileNameFormat = "")
        {
            if (!string.IsNullOrEmpty(logpath)) _logpath = logpath;
            if (!string.IsNullOrEmpty(fileNameFormat)) _fileNameFormat = fileNameFormat;
        }

        public void Write(string content, bool hasTimeStamp, FileTypeEnum fileType)
        {
            try
            {
                lock (locker)
                {
                    IFileNameGenerator generator = FileNameGeneratorFactory.Create(fileType);
                    string fullfilename = generator.GetFileName(_fileNameFormat, fileSize);
                    FileInfo file = new FileInfo(fullfilename);
                    if (!file.Directory.Exists) file.Directory.Create();
                    StringBuilder log = new StringBuilder();
                    if (hasTimeStamp) log.AppendFormat("{0} -- {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
                    else log.Append(content);
                    //using (FileStream logFile = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    //{
                    //    var logContentBytes = Encoding.UTF8.GetBytes(log.ToString());
                    //    logFile.Seek(0, SeekOrigin.End);
                    //    logFile.Write(logContentBytes, 0, logContentBytes.Length);
                    //}
                    m.WaitOne();
                    File.AppendAllLines(file.FullName, new string[] { log.ToString() });
                    m.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Write(string content, bool hasTimeStamp)
        {
            Write(content, hasTimeStamp, FileTypeEnum.Log);
        }

        public void Write(string content)
        {
            Write(content, true);
        }

        /// <summary>
        /// 写错误信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="hasTimeStamp"></param>
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
            this.Write(err.ToString(), hasTimeStamp, FileTypeEnum.Error);
        }

        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void Log(string content, bool hasTimeStamp = true)
        {
            Write(content, hasTimeStamp, FileTypeEnum.Log);
        }

        /// <summary>
        /// 写错误信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void Error(string content, bool hasTimeStamp = true)
        {
            Write(content, hasTimeStamp, FileTypeEnum.Error);
        }

        /// <summary>
        /// 写调试信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void Debug(string content, bool hasTimeStamp = true)
        {
            Write(content, hasTimeStamp, FileTypeEnum.Debug);
        }
    }
}
