using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class TypePool
    {
        private static readonly ConcurrentDictionary<string, Type> pool = new ConcurrentDictionary<string, Type>();
        private static bool init = false;

        private static SimpleLogger logger = new SimpleLogger();

        public static void Init()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            List<FileInfo> fileList = new List<FileInfo>();
            fileList.AddRange(dir.GetFiles("*.dll"));
            DirectoryInfo bin = new DirectoryInfo(dir.FullName + "bin");
            fileList.AddRange(bin.GetFiles("*.dll"));
            foreach (var f in fileList)
            {
                try
                {
                    if (f.Name.ToLower().StartsWith("system.") || f.Name.ToLower().StartsWith("microsoft.")) continue;
                    var ass = Assembly.LoadFile(f.FullName);
                    var types = ass.GetTypes();
                    foreach (var t in types)
                    {
                        if (t.FullName.StartsWith("<")) continue;
                        pool[t.FullName] = t;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(f.FullName);
                    logger.WriteException(ex);
                }
            }
        }

        public static Type GetType(string fullTypeName)
        {
            Type t = null;
            if (!init)
            {
                init = true;
                Init();
            }
            if (pool.ContainsKey(fullTypeName))
            {
                pool.TryGetValue(fullTypeName, out t);
            }
            return t;
        }
    }
}
