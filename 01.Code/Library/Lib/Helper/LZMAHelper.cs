using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Athena.Unitop.Sure.Lib
{
    public class LZMAHelper
    {
        /// <summary>
        /// LZMA压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Compress(string value)
        {
            return "";
        }

        /// <summary>
        /// LZMA解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnCompress(string value)
        {
            byte[] byteArray = Convert.FromBase64String(value);
            byte[] tmpArray;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream inStream = new MemoryStream(byteArray))
                {
                    byte[] properties = new byte[5];
                    if (inStream.Read(properties, 0, 5) != 5)
                        throw (new Exception("input .lzma is too short"));
                    SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
                    decoder.SetDecoderProperties(properties);

                    long outSize = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int v = inStream.ReadByte();
                        if (v < 0)
                            throw (new Exception("Can't Read 1"));
                        outSize |= ((long)(byte)v) << (8 * i);
                    }
                    long compressedSize = inStream.Length - inStream.Position;
                    decoder.Code(inStream, outStream, compressedSize, outSize, null);
                    tmpArray = outStream.ToArray();

                }
            }
            return Encoding.UTF8.GetString(tmpArray);
        }
    }
}
