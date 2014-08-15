using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;

namespace SOAFramework.Library
{
    public class FTPClient
    {
        #region attribute
        private string _str_LocalFileName;
        private string _str_FtpUrl;
        private string _str_LocalFullFileName;
        private string _str_ExInfoFileName;//额外信息文件的名字
        private string _str_LocalFilePath;
        private string _str_UserName;
        private string _str_Password;
        private string _str_FtpFileUrl;
        private int _int_BufferSize = 1024;
        private bool _bl_ContinueFromBreak = true;
        #endregion

        #region property
        /// <summary>
        /// 存储在本地的文件路径
        /// </summary>
        public string LocalFilePath
        {
            get { return _str_LocalFilePath; }
            set 
            {
                _str_LocalFilePath = value;
                CombineProperty();
            }
        }
        /// <summary>
        /// FTP用户名
        /// </summary>
        public string UserName
        {
            set { _str_UserName = value; }
            get { return _str_UserName; }
        }
        /// <summary>
        /// FTP密码
        /// </summary>
        public string Password
        {
            set { _str_Password = value; }
            get { return _str_Password; }
        }
        /// <summary>
        /// 是否断点续传
        /// </summary>
        public bool ContinueFromBreak
        {
            get { return _bl_ContinueFromBreak; }
            set { _bl_ContinueFromBreak = value; }
        }
        /// <summary>
        /// FTP url
        /// </summary>
        public string FtpUrl
        {
            get { return _str_FtpUrl; }
            set 
            { 
                _str_FtpUrl = value;
                CombineProperty();
            }
        }
        /// <summary>
        /// buff宽度
        /// </summary>
        public int BufferSize
        {
            get { return _int_BufferSize; }
            set { _int_BufferSize = value; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return _str_LocalFileName; }
            set
            {
                _str_LocalFileName = value;
                CombineProperty();
            }
        }
        #endregion

        #region operation
        /// <summary>
        /// 从FTP服务器上下载文件
        /// </summary>
        public void Download()
        {
            FileStream fsWriter = null;
            long lngReadPosition = 0;
            long lngBufferPage = 0;
            FtpWebResponse fwrResponse = null;
            Stream stream = null;
            FtpWebRequest fwrRequest = WebRequest.Create(_str_FtpFileUrl) as FtpWebRequest;
            try
            {
                NetworkCredential nc = new NetworkCredential(_str_UserName, _str_Password);
                fwrRequest.Credentials = nc;
                fwrRequest.UseBinary = true;
                fwrResponse = fwrRequest.GetResponse() as FtpWebResponse;
                stream = fwrResponse.GetResponseStream();
                long lngFileLength = fwrResponse.ContentLength;
                fsWriter = File.OpenWrite(_str_LocalFullFileName);
                if (_bl_ContinueFromBreak)
                {
                    lngReadPosition = ReadProcessedSize();
                }
                while (lngReadPosition < lngFileLength)
                {
                    lngReadPosition = lngBufferPage * _int_BufferSize;
                    byte[] bytBuffer = new byte[_int_BufferSize];
                    stream.Position = lngReadPosition;
                    stream.Read(bytBuffer, 0, _int_BufferSize);
                    fsWriter.Write(bytBuffer, 0, bytBuffer.Length);
                    lngBufferPage++;
                }
            }
            finally
            {
                if (fwrResponse != null)
                {
                    fwrResponse.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (fsWriter != null)
                {
                    fsWriter.Close();
                }
            }
        }
        #endregion

        #region helper method
        private void CombineProperty()
        {
            if (!string.IsNullOrEmpty(_str_FtpUrl) && !string.IsNullOrEmpty(_str_LocalFileName))
            {
                _str_FtpFileUrl = _str_FtpUrl + "/" + _str_LocalFileName;
            }
            if (!string.IsNullOrEmpty(_str_LocalFilePath) && !string.IsNullOrEmpty(_str_LocalFileName))
            {
                _str_LocalFullFileName = _str_LocalFilePath.EndsWith(@"\") ? _str_LocalFilePath + _str_LocalFileName : _str_LocalFilePath + @"\" + _str_LocalFileName;
                _str_ExInfoFileName = _str_LocalFileName + ".dt";
            }
        }

        private long ReadProcessedSize()
        {
            if (!File.Exists(_str_ExInfoFileName))
            {
                return 0;
            }
            string strContent = File.ReadAllText(_str_ExInfoFileName, Encoding.UTF8);
            long lngProcessedSize = 0;
            long.TryParse(strContent, out lngProcessedSize);
            return lngProcessedSize;
        }

        private void WriteProcessedSize(long ProcessedSize)
        {
            byte[] bytProcessedSize = Encoding.UTF8.GetBytes(ProcessedSize.ToString());
            File.OpenWrite(_str_ExInfoFileName).Write(bytProcessedSize, 0, bytProcessedSize.Length);
        }
        #endregion
    }
}
