using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SOAFramework.Library
{
    /// <summary>
    /// 用于系统记录日志
    /// Create By CZQ
    /// </summary>
    public class AppLog
    {
        static string _logPath = string.Empty;
        //为1表示记录普通日志，为0表示不记录普通日志
        static int _logLevel = 1;

        #region 放弃单例模式，改用静态方法
        static AppLog()
        {
            _logPath = string.Format("{0}log\\", AppDomain.CurrentDomain.BaseDirectory);
        }

        AppLog()
        { }

        //static readonly AppLog _instance = new AppLog();     

        //public static AppLog Instance
        //{
        //    get
        //    {
        //        return _instance;
        //    }
        //}
        #endregion

        public static void WriteLog(string log, string fileName = "")
        {
            Write(log, LogType.DEBUG, fileName);
        }

        public static void WriteError(string error, string fileName = "")
        {
            Write(error, LogType.ERROR, fileName);
        }

        public static void Write(string message, LogType type = LogType.DEBUG, string fileName = "")
        {
            if (_logLevel == 0 && type == LogType.DEBUG)
            {
                return;
            }

            try
            {
                StreamWriter sw = new StreamWriter(GetLogFilePath(type, fileName), true);
                sw.WriteLine(
                    string.Format("{0} [{1}] \r\n{2}",
                    type.ToLocalString(),
                    DateTime.Now.ToString(),
                    message));
                sw.WriteLine(GetSeparatorStr());
                sw.Close();
            }
            catch { }
        }

        public static void WriteError(Exception ex, string fileName = "")
        {
            string errorMsg = GetExceptionStr(ex);
            Write(errorMsg, LogType.ERROR, fileName);
        }

        /// <summary>
        /// 获取文件的完整路径
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        static string GetLogFilePath(LogType type, string fileName)
        {
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
            if (fileName.Length == 0)
            {
                fileName = DateTime.Now.ToString("yyyyMMdd");
            }
            string filePath = string.Format("{0}{1}.log", _logPath, fileName);

            if (!File.Exists(filePath))
            {
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine("**************************************************************************************************");
                sw.Close();
            }
            return filePath;
        }

        static string GetSeparatorStr()
        {
            return "----------------------------------------------------------------------------------------------------------------------------";
        }

        static string GetExceptionStr(Exception exp)
        {
            string innerException = exp.InnerException == null ? string.Empty : exp.InnerException.Message;
            string error = string.Format(
                    "异常信息:{0}\r\nInnerException信息:{1}\r\n帮助文件链接:{2}\r\n导致错误的应用程序或对象的名称:{3}\r\n堆栈信息:{4}\r\n引发异常的方法:{5}",
                    exp.Message,
                    innerException,
                    exp.HelpLink,
                    exp.Source,
                    exp.StackTrace,
                    exp.TargetSite);
            return error;
        }
    }

    /// <summary>
    /// 日志类别
    /// </summary>
    public enum LogType
    {
        ERROR,
        DEBUG
    }

    public static class LogTypeExtension
    {
        public static string ToLocalString(this LogType type, string localStr = "zh-cn")
        {
            switch (localStr)
            {
                case "zh-cn":
                    switch (type)
                    {
                        case LogType.ERROR:
                            return "error";
                        case LogType.DEBUG:
                        default:
                            return "debug";
                    }
                default:
                    return localStr.ToString();
            }
        }
    }
}
