using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public abstract class BaseFTPUrl
    {
        public abstract string Prefix { get; }

        public string Url
        {
            get
            {
                return GetUrl();
            }
            set
            {
                string noPrefix = value.Replace(Prefix + "://", "");
                int first = noPrefix.IndexOf("/");
                if (first > -1)
                {
                    IP = noPrefix.Remove(first);
                    string noIp = noPrefix.Substring(first + 1);
                    Path = noIp;
                }
                else
                {
                    IP = noPrefix;
                }
            }
        }

        private enumPathType pathType = enumPathType.文件;
        public enumPathType PathType 
        { 
            get{return pathType;}
        }

        public string IP { get; set; }

        public virtual int Port { get; set; }

        private string directory;
        public string Directory 
        {
            get { return directory; }
        }

        private string path;
        public string Path 
        { 
            get
            {
                return path;
            }
            set
            {
                path = value;
                int lastPoint = value.LastIndexOf(".");
                int last = value.LastIndexOf("/");
                if (lastPoint > last)
                {
                    pathType = enumPathType.文件;
                    if (last > -1)
                    {
                        directory = value.Remove(last);
                    }
                    fileName = value.Substring(last + 1);
                }
                else
                {
                    directory = value;
                    fileName = null;
                    pathType = enumPathType.目录;
                }
            }
        }

        private string fileName;
        public string FileName 
        {
            get { return fileName; }
        }

        protected string GetUrl()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}://{1}:{2}/", Prefix, IP, Port);
            if (!string.IsNullOrEmpty(directory))
            {
                builder.Append(directory.TrimEnd('/')).Append("/");
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                builder.Append(fileName);
            }
            return builder.ToString();
        }
    }
}
