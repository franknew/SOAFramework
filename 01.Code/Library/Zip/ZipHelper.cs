using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class ZipHelper
    {

        public static byte[] ZipToByte(string value, Encoding encode, bool useGzip = false)
        {
            byte[] byteArray = encode.GetBytes(value);
            byte[] data = null;
            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            if (useGzip)
            {
                System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                //Compress
                sw.Write(byteArray, 0, byteArray.Length);
                //Close, DO NOT FLUSH cause bytes will go missing...
                sw.Close();
                sw.Dispose();
            }
            else
            {
                ms.Write(byteArray, 0, byteArray.Length);
            }
            data = ms.ToArray();
            //Transform byte[] zip data to string
            ms.Close();
            ms.Dispose();
            return data;
        }


        public static string Zip(string value)
        {
            return Zip(value, Encoding.Default);
        }

        public static string Zip(string value, Encoding encode)
        {
            //Transform string into byte[]  
            byte[] byteArray = ZipToByte(value, encode);
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            return sB.ToString();
        }

        public static string UnZip(string value)
        {
            System.Text.StringBuilder sB = new StringBuilder();
            //Transform string into byte[]
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for decompress
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            List<byte> byteList = new List<byte>();
            //Reset variable to collect uncompressed result
            //byteArray = new byte[byteArray.Length * 3 ];
            int data = sr.ReadByte();
            while (data != -1)
            {
                //sB.Append((char)data);
                byteList.Add((byte)data);
                data = sr.ReadByte();
            }
            //Decompress
            sB.Append(Encoding.UTF8.GetString(byteList.ToArray()));
            //Transform byte[] unzip data to string
            //Read the number of bytes GZipStream red and do not a for each bytes in
            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static byte[] DecodeBase64(string value)
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

        public static string EncodeBase64(string value, Encoding encode)
        {
            string data = null;
            try
            {
                byte[] bytes = encode.GetBytes(value);
                data = Convert.ToBase64String(bytes);
            }
            catch (FormatException e)
            {
                data = value;
            }
            return data;
        }

        public static byte[] UnZipString(string value, bool useGzip = false)
        {
            bool isuncompress = true;
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("待解压缩的字符串不能为空!");

            byte[] byteArray = DecodeBase64(value);
            if (!isuncompress)
            {
                return byteArray;
            }
            byte[] tmpArray;
            using (MemoryStream msOut = new MemoryStream())
            {
                using (MemoryStream msIn = new MemoryStream(byteArray))
                {
                    if (useGzip)
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
                    else
                    {
                        byte[] bytes = new byte[40960];
                        int n;
                        while ((n = msIn.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            msOut.Write(bytes, 0, n);
                        }
                        msIn.Close();
                        tmpArray = msOut.ToArray();
                    }
                }
            }
            return tmpArray;
        }

        public static void UnZipToFile(string value, string fileName, bool useGzip = false)
        {
            var data = UnZipString(value, useGzip);
            FileInfo file = new FileInfo(fileName);
            FileStream stream = null;
            if (!file.Exists) stream = file.Create();
            else stream = file.OpenWrite();
            stream.Write(data, 0, data.Length);
            stream.Close();

            //File.WriteAllBytes(file.FullName, data);
        }

        public static string ZipFileToString(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            return Convert.ToBase64String(bytes);
        }

        public static string ZipParallel(string value, Encoding coding)
        {
            object locker = new object();
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
            Parallel.For(0, value.Length, parallelOptions, t =>
            {

            });
            return null;
        }

        public static void PackageFiles(string fullFileName, IEnumerable<string> fileNames)
        {
            FileInfo file = new FileInfo(fullFileName);
            Stream stream = null;
            if (!file.Directory.Exists) file.Directory.Create();
            if (!file.Exists) stream = file.Create();
            else stream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            ZipFile zip = new ZipFile(stream);
            try
            {
                zip.BeginUpdate();
                foreach (var key in fileNames)
                {
                    FileInfo f = new FileInfo(key);
                    zip.Add(key, f.Name);
                }
                zip.CommitUpdate();
            }
            finally
            {
                stream?.Close();
                zip?.Close();
            }
        }

        public static void PackageDirectory(string fullFileName, string directory, string searchPattern = null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = null;
            if (!string.IsNullOrEmpty(searchPattern)) files = directoryInfo.GetFiles(searchPattern);
            else files = directoryInfo.GetFiles();
            var fileNames = (from f in files select f.FullName).ToList();
            PackageFiles(fullFileName, fileNames);
        }
    }
}
