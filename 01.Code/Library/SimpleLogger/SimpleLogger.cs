using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class SimpleLogger
    {
        private string _logpath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        private string _fileNameFormat = "yyyyMMdd";
        private static object locker = new object();

        static ReaderWriterLockSlim writeLock = new ReaderWriterLockSlim();
        public static long fileSize = 5 * 1024 * 1024;
        

        public SimpleLogger(string logpath = "", string fileNameFormat = "")
        {
            if (!string.IsNullOrEmpty(logpath)) _logpath = logpath;
            if (!string.IsNullOrEmpty(fileNameFormat)) _fileNameFormat = fileNameFormat;
        }

        public void Write(string content, bool hasTimeStamp, FileTypeEnum fileType)
        {
            try
            {
                IFileNameGenerator generator = FileNameGeneratorFactory.Create(fileType);
                string fullfilename = generator.GetFileName(_fileNameFormat, fileSize);
                FileInfo file = new FileInfo(fullfilename);
                StringBuilder log = new StringBuilder();
                if (hasTimeStamp) log.AppendFormat("{0} -- {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
                else log.Append(content);
                //lock (locker)
                //{
                    if (!writeLock.IsWriteLockHeld) writeLock.EnterWriteLock();
                    try
                    {
                        if (!file.Exists) file.Create().Close();
                        File.AppendAllLines(file.FullName, new string[] { log.ToString() });
                    }
                    finally
                    {
                        if (writeLock.IsWriteLockHeld) writeLock.ExitWriteLock();
                    }
                //}
            }
            catch 
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

        /// <summary>
        /// 异步写日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        /// <param name="fileType"></param>
        public void WriteAsync(string content, bool hasTimeStamp, FileTypeEnum fileType)
        {
            Task task = new Task(() =>
            {
                Write(content, hasTimeStamp, fileType);
            });
            task.Start();
        }

        /// <summary>
        /// 异步写日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void LogAsync(string content, bool hasTimeStamp = true)
        {
            WriteAsync(content, hasTimeStamp, FileTypeEnum.Log);
        }

        /// <summary>
        /// 异步写错误信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void ErrorAsync(string content, bool hasTimeStamp = true)
        {
            WriteAsync(content, hasTimeStamp, FileTypeEnum.Error);
        }

        /// <summary>
        /// 异步写调试信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasTimeStamp"></param>
        public void DebugAsync(string content, bool hasTimeStamp = true)
        {
            WriteAsync(content, hasTimeStamp, FileTypeEnum.Debug);
        }

        /// <summary>
        /// 异步写异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="hasTimeStamp"></param>
        public void WriteExceptionAsync(Exception ex, bool hasTimeStamp = true)
        {

            Task task = new Task(() =>
            {
                WriteException(ex, hasTimeStamp);
            });
            task.Start();
        }
    }
}
