using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class _7ZipHelper
    {
        private const string zipMark = "*zip*";
        public static string Zip(string value)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            return ZipByte(byteArray);
        }

        public static string ZipByte(byte[] byteArray)
        {

            byte[] tmpArray;

            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream sw = new GZipStream(ms, CompressionMode.Compress))
                {

                    sw.Write(byteArray, 0, byteArray.Length);
                    sw.Flush();

                }
                tmpArray = ms.ToArray();
            }
            string zipstring = Convert.ToBase64String(tmpArray);
            return zipMark + zipstring + zipMark;
        }
        /// <summary>
        /// 获取base64字符串的字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetByteFromBase64(string value)
        {
            try
            {
                return Convert.FromBase64String(value);
            }
            catch (FormatException e)
            {
                return Encoding.Default.GetBytes(value);
            }
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnZip(string value)
        {

            byte[] tmpArray = UnZipFile(value);
            if (tmpArray != null)
                return Encoding.GetEncoding("UTF-8").GetString(tmpArray);
            else return "";
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] UnZipFile(string value)
        {
            //如果value前后都包含athena表示无压缩，执行直接返回字节即可
            bool isuncompress = true;
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("待解压缩的字符串不能为空!");
            if (value.StartsWith(zipMark) && value.EndsWith(zipMark))
            {
                isuncompress = false;
                value = value.Replace(zipMark, "");
            }

            byte[] byteArray = GetByteFromBase64(value);
            if (!isuncompress)
            {
                return byteArray;
            }
            byte[] tmpArray;
            using (MemoryStream msOut = new MemoryStream())
            {
                using (MemoryStream msIn = new MemoryStream(byteArray))
                {
                    using (GZipStream swZip = new GZipStream(msIn, CompressionMode.Decompress, true))
                    {
                        byte[] bytes = new byte[40960];
                        int n;
                        while ((n = swZip.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            msOut.Write(bytes, 0, n);
                        }
                        swZip.Close();
                        tmpArray = msOut.ToArray();

                    }
                }
            }
            return tmpArray;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static string ZipFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException("文件不存在");
            FileInfo fi = new FileInfo(filename);
            long len = fi.Length;
            Stream stream2 = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[len];
            stream2.Read(buffer, 0, (int)len);
            stream2.Close();
            string str = _7ZipHelper.ZipByte(buffer);
            return str;
        }

    }
}
