using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Tamir.SharpSsh.jsch;

namespace SOAFramework.Library
{
    public class SFTPClient : IFTPClient
    {
        #region 构造行数
        public SFTPClient(string ip = null, int port = 22, string filePath = null, string username = null, string password = null)
        {
            UrlEntity = new SFTPUrl { IP = ip, Port = port, Path = filePath };
            this.username = username;
            this.password = password;
            JSch jsch = new JSch();
            session = jsch.getSession(username, ip, port);
            STFPUserInfo userInfo = new STFPUserInfo(password);
            session.setUserInfo(userInfo);
        }

        public SFTPClient(string url, string username = null, string password = null)
        {
            UrlEntity = new SFTPUrl { Url = url };
            this.username = username;
            this.password = password;
            JSch jsch = new JSch();
            session = jsch.getSession(username, UrlEntity.IP, UrlEntity.Port);
            STFPUserInfo userInfo = new STFPUserInfo(password);
            session.setUserInfo(userInfo);
        }
        #endregion

        #region 属性
        private Session session;

        private Channel channel;

        private ChannelSftp sftp;

        private string mappingLocalDirectory = AppDomain.CurrentDomain.BaseDirectory + "Download\\";

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

        #region 公共行为
        /// <summary>
        /// 获得文件夹内所有文件名
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileNames()
        {
            List<string> list = new List<string>();
            string newPath = UrlEntity.Path;
            if (UrlEntity.PathType == enumPathType.文件)
            {
                newPath = UrlEntity.Directory;
            }
            string url = "/" + newPath.TrimStart('/');
            ArrayList array = sftp.ls(url);
            foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry data in array)
            {
                string name = data.getFilename();
                if (name.Trim() != "." && name.Trim() != "..")
                {
                    list.Add(data.getFilename());
                }
            }
            return list;
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
                string url = UrlEntity.Path;
                listDownloadFiles.Add(UrlEntity.FileName);
            }
            else
            {
                List<string> ftpFileNames = GetFileNames();
                ftpFileNames.ForEach(t =>
                {
                    string url = t.Trim();
                    listDownloadFiles.Add(url);
                });
            }
            listDownloadFiles.ForEach(t =>
            {
                string fileName = UrlEntity.Directory + t;
                string localFullFileName = mappingLocalDirectory.TrimEnd('\\') + "\\" + t;
                DownloadFile(fileName, localFullFileName, overwrite);
            });
            return true;
        }

        public string DownloadFile(string fileUrl, string saveFilaFullName, bool overwrite = false)
        {
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
            Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(fileUrl);
            Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(localFullFileName);
            sftp.get(src, dst);
            return localFullFileName;
        }

        public bool DeleteFile()
        {
            switch (UrlEntity.PathType)
            {
                case enumPathType.目录:
                    List<string> list = GetFileNames();
                    list.ForEach(t =>
                    {
                        sftp.rm(UrlEntity.Directory + t);
                    });
                    break;
                default:
                    sftp.rm(UrlEntity.Directory + UrlEntity.FileName);
                    break;
            }
            return true;
        }

        public bool DeleteFile(string path)
        {
            sftp.rm(path);
            return true;
        }

        public bool Connect()
        {
            if (!session.isConnected())
            {
                session.connect();
                channel = session.openChannel("sftp");
                channel.connect();
                sftp = (ChannelSftp)channel;
            }
            return true;
        }

        public void Disconnect()
        {
            if (session.isConnected())
            {
                channel.disconnect();
                session.disconnect();
            }
        }
        #endregion
    }

    public class STFPUserInfo : UserInfo
    {
        public STFPUserInfo(string password)
        {
            this.password = password;
        }

        private string password;
        public string getPassphrase()
        {
            return "";
        }
        public string getPassword()
        {
            return password;
        }
        public bool promptPassphrase(string message)
        {
            return true;
        }
        public bool promptPassword(string message)
        {
            return true;
        }
        public bool promptYesNo(string message)
        {
            return true;
        }
        public void showMessage(string message)
        {

        }
    }
}
