using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface IFTPClient
    {
        BaseFTPUrl UrlEntity { get; set; }

        string Username { get; set; }

        string Password { get; set; }


        string FileDirectory { get; }

        string MappingLocalDirectory { get; set; }


        List<string> GetFileNames();


        bool Download(bool overwrite = false);


        string DownloadFile(string fileUrl, string saveFilaFullName, bool overwrite = false);

        bool DeleteFile();

        bool DeleteFile(string url);

        bool Connect();

        void Disconnect();
    }

    public enum FTPType
    {
        FTP,
        SFTP,
    }
}
