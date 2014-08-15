using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using Frank.Common.DAL;

namespace SOAFramework.Controller
{
    public class Bussiness 
    {
        public string Test(string a)
        {
            return "Hello," + a;
        }

        public byte[] GetFileStream(int CurrentIndex, long Size, string FullFileName)
        {
            FileStream fsReader = null;
            long intCount = Size;
            byte[] bytFileStream = null;
            long intStartIndex = (CurrentIndex - 1) * Size;
            long intEndIndex = CurrentIndex * Size - 1;
            FileInfo fiTemp = new FileInfo(FullFileName);
            int intFileLength = (int)fiTemp.Length;
            if (intEndIndex > intFileLength)
            {
                intCount = intFileLength - intStartIndex;
            }
            try
            {
                bytFileStream = new byte[intCount];
                fsReader = fiTemp.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fsReader.Position = intStartIndex;
                fsReader.Read(bytFileStream, 0, (int)intCount);
            }
            finally
            {
                if (null != fsReader)
                {
                    fsReader.Close();
                }
            }
            return bytFileStream;
        }

        public DataTable RunSQLWithTable(string sql)
        {
            DBHelper helper = DBFactory.CreateDBHelper();
            DataTable table = helper.GetTableWithSQL(sql);
            return table;
        }
    }
}
