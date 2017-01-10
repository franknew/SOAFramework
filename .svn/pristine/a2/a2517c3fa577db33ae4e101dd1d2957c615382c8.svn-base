using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class LogHelper
    {
        public static void WriteLog(string log)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder filepath = new StringBuilder();
            sql.AppendFormat("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log);
            filepath.AppendFormat(@"{0}Logs\log{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
            FileInfo file = new FileInfo(filepath.ToString());
            if (!file.Directory.Exists) file.Directory.Create();
            File.AppendAllLines(filepath.ToString(), new List<string> { sql.ToString() });
        }
    }
}
