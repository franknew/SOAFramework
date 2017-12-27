using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class AssemblyHelper
    {
        /// <summary>
        /// 加载复制版本
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static Assembly LoadCopy(string fullFileName)
        {
            byte[] assbyte = null;
            using (FileStream stream = new FileStream(fullFileName, FileMode.Open))
            {
                assbyte = new byte[stream.Length];
                stream.Read(assbyte, 0, (int)stream.Length);
            }
            return Assembly.Load(assbyte);
        }
    }
}
