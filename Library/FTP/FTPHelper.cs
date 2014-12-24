using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class FTPHelper
    {

        public static IFTPClient CreateInstance(string ip, int port, string path, string username, string password, FTPType type)
        {
            IFTPClient client;
            switch (type)
            {
                case FTPType.SFTP:
                    client = new SFTPClient(ip, port, path, username, password);
                    break;
                default:
                    client = new FTPClient(ip, port, path, username, password);
                    break;
            }
            return client;
        }
    }
}
