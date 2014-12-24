using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class ZipHelper
    {
        private const string zipMark = "#zip#";

        public static byte[] ZipToByte(string value)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            int indexBA = 0;

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return ms.ToArray();
        }

        public static string Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = ZipToByte(value);
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            sB.Insert(0, zipMark);
            sB.Append(zipMark);
            return sB.ToString();
        }

        public static string UnZip(string value)
        {
            System.Text.StringBuilder sB = new StringBuilder();
            if (value.StartsWith(zipMark) && value.EndsWith(zipMark))
            {
                value = value.Replace(zipMark, "");
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
            }
            return sB.ToString();
        }
    }
}
