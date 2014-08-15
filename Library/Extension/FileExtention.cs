using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public static class FileExtention
    {
        public static string FileToString(this FileInfo file)
        {
            FileStream stream = new FileStream(file.FullName,
                  System.IO.FileMode.Open, System.IO.FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            byte[] bytFile = new byte[file.Length];
            reader.Read(bytFile, 0, bytFile.Length);
            string fileString = Convert.ToBase64String(bytFile);
            stream.Close();
            reader.Close();
            return fileString;
        }

        public static void ToFile(this String fileString, string fullFileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            //实例化一个用于写的BinaryWriter  
            bw.Write(Convert.FromBase64String(fileString));
            bw.Close();
            fs.Close();
        }
    }
}
