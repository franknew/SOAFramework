using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Athena.Unitop.Monitor.Lib
{
    public class FTPClient : IFTPClient
    {
        #region 属性
        private string mappingLocalDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Download\\";


        private string fileDirectory;

        public string FileDirectory
        {
            get { return fileDirectory; }
        }

        public string MappingLocalDirectory
        {
            get { return mappingLocalDirectory; }
            set { mappingLocalDirectory = value; }
        }

        public BaseFTPUrl UrlEntity { get; set; }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        #region 构造方法
        public FTPClient(string ip = null, int port = 21, string filePath = null, string username = null, string password = null)
        {
            UrlEntity = new FTPUrl { IP=ip, Port = port, Path = filePath };
            Username = username;
            Password = password;
        }

        public FTPClient(string url, string username = null, string password = null)
        {
            UrlEntity = new FTPUrl { Url = url };
            Username = username;
            Password = password;
        }
        #endregion

        #region 公共行为
        /// <summary>
        /// 获得文件夹内所有文件名
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileNames()
        {
            StreamReader reader = null;
            WebResponse response = null;
            try
            {
                List<string> list = new List<string>();
                string newPath = UrlEntity.Path;
                if (UrlEntity.PathType == enumPathType.文件)
                {
                    newPath = UrlEntity.Directory;
                }
                string url = "ftp://" + UrlEntity.IP + ":" + UrlEntity.Port.ToString() + "/" + newPath.TrimStart('/');
                response = Request(url, WebRequestMethods.Ftp.ListDirectoryDetails);
                reader = new StreamReader(response.GetResponseStream());
                string ftpContent = reader.ReadToEnd();
                List<string> listFileInfo = ftpContent.Split('\r', '\n').ToList();
                listFileInfo.RemoveAll(t => t == "");
                listFileInfo.ForEach(t =>
                {
                    list.Add(t.Substring(t.LastIndexOf(" "), t.Length - t.LastIndexOf(" ")));
                });
                return list;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 获得文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileContent(string url)
        {
            Stream stream = null;
            string content = null;
            FtpWebResponse response = null;
            try
            {
                response = Request(url) as FtpWebResponse;
                stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GBK"));
                content = reader.ReadToEnd();
                return content;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveFullFileName"></param>
        public bool Download(bool overwrite = false)
        {
            List<string> listDownloadFiles = new List<string>();
            if (UrlEntity.PathType == enumPathType.文件)
            {
                listDownloadFiles.Add(UrlEntity.Url);
            }
            else
            {
                List<string> ftpFileNames = GetFileNames();
                ftpFileNames.ForEach(t =>
                {
                    string url = UrlEntity.Url + t.Trim();
                    listDownloadFiles.Add(url);
                });
            }
            listDownloadFiles.ForEach(t =>
            {
                string fileName = t.Substring(t.LastIndexOf('/') + 1, t.Length - t.LastIndexOf('/') - 1);
                string localFullFileName = mappingLocalDirectory.TrimEnd('\\') + "\\" + fileName;
                DownloadFile(fileName, localFullFileName, overwrite);
            });
            return true;
        }

        public string DownloadFile(string fileUrl, string saveFilaFullName, bool overwrite = false)
        {
            string content = GetFileContent(fileUrl);

            FileInfo file = new FileInfo(saveFilaFullName);
            string localFullFileName = saveFilaFullName;
            int i = 1;
            if (!overwrite)
            {
                do
                {
                    if (file.Exists)
                    {
                        localFullFileName = saveFilaFullName.Remove(saveFilaFullName.LastIndexOf(".")) + "_" + i.ToString() + file.Extension;
                    }
                    file = new FileInfo(localFullFileName);
                    i++;
                } while (file.Exists);
            }
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (!file.Exists)
            {
                file.Create().Close();
            }
            File.WriteAllText(localFullFileName, content);
            return localFullFileName;
        }

        public WebResponse Request(string url, string method = WebRequestMethods.Ftp.DownloadFile)
        {
            FtpWebRequest request = FtpWebRequest.Create(url) as FtpWebRequest;
            request.UseBinary = true;
            request.Proxy = null;
            request.Method = method;
            request.Credentials = new NetworkCredential(username, password);
            request.KeepAlive = false;
            return request.GetResponse();
        }

        public bool DeleteFile()
        {
            List<string> listDownloadFiles = new List<string>();
            if (UrlEntity.PathType == enumPathType.文件)
            {
                listDownloadFiles.Add(UrlEntity.Url);
            }
            else
            {
                List<string> ftpFileNames = GetFileNames();
                ftpFileNames.ForEach(t =>
                {
                    string url = UrlEntity.Url + t.Trim();
                    listDownloadFiles.Add(url);
                });
            }
            listDownloadFiles.ForEach(t =>
            {
                Request(t, WebRequestMethods.Ftp.DeleteFile);
            });
            return true;
        }

        public bool DeleteFile(string url)
        {
            FTPClient client = new FTPClient(url, username, password);
            client.DeleteFile();
            return true;
        }

        public bool Connect()
        {
            return true;
        }

        public void Disconnect()
        {

        }
        #endregion
    }

    public enum enumPathType
    {
        文件,
        目录
    }
}
