using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    class ErrorFileNameGenerator : IFileNameGenerator
    {
        private static int _index = 0;
        private static DateTime _logTime = DateTime.Now;
        private string _logpath = AppDomain.CurrentDomain.BaseDirectory + "Error";

        public string GetFileName(string fileNameFormat, long fileSize)
        {
            if (_logTime.Year != DateTime.Now.Year || _logTime.Month != DateTime.Now.Month || _logTime.Day != DateTime.Now.Day)
            {
                _logTime = DateTime.Now;
                _index = 0;
            }
            string fullfilename = _logpath.TrimEnd('\\') + "\\" + _logTime.ToString(fileNameFormat) + (_index > 0 ? _index.ToString("000") : "") + ".err";
            FileInfo file = new FileInfo(fullfilename);
            if (!file.Exists) return fullfilename;
            if (file.Length >= fileSize)
            {
                _index++;
                return GetFileName(fileNameFormat, fileSize);
            }

            return fullfilename;
        }
    }
}
