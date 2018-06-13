using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public abstract class BaseFileNameGenerator: IFileNameGenerator
    {
        private static DateTime _logTime = DateTime.Now;
        private string _logpath = null;
        private static object locker = new object();

        public BaseFileNameGenerator()
        {
            _logpath = AppDomain.CurrentDomain.BaseDirectory + GetDirectory();
        }

        public string GetFileName(string fileNameFormat, long fileSize)
        {
            if (_logTime.Year != DateTime.Now.Year || _logTime.Month != DateTime.Now.Month || _logTime.Day != DateTime.Now.Day)
            {
                _logTime = DateTime.Now;
                SetIndex(0);
            }
            string fullfilename = _logpath.TrimEnd('\\') + "\\" + _logTime.ToString(fileNameFormat) + (GetIndex() > 0 ? GetIndex().ToString("000") : "") + GetExtension();
            FileInfo file = new FileInfo(fullfilename);
            bool overFitSize = false;
            lock (locker)
            {
                overFitSize = file.Exists ? file.Length >= fileSize : false;
                if (overFitSize) IncreaseIndex();
            }
            if (overFitSize) return GetFileName(fileNameFormat, fileSize);

            return fullfilename;
        }

        public abstract string GetDirectory();

        public abstract string GetExtension();

        public abstract void IncreaseIndex();

        public abstract int GetIndex();

        public abstract void SetIndex(int index);
    }
}
