using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace SOAFramework.Library.Lib
{
   public class ZipHelper
    {

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
            return Convert.ToBase64String(tmpArray);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnZip(string value)
        {
            byte[] byteArray = Convert.FromBase64String(value);
            byte[] tmpArray = UnZipFile(value);
            if (tmpArray != null)
                return Encoding.GetEncoding("UTF-8").GetString(tmpArray);
            else return "";
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
            string str = ZipHelper.ZipByte(buffer);
            return str;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] UnZipFile(string value)
        {
            byte[] byteArray = Convert.FromBase64String(value);
            byte[] tmpArray;

            using (MemoryStream msOut = new MemoryStream())
            {
                using (MemoryStream msIn = new MemoryStream(byteArray))
                {
                    using (GZipStream swZip = new GZipStream(msIn, CompressionMode.Decompress))
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
    }
}
