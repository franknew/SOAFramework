using SOAFramework.Server.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOAFramework.Server.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerUI());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = new AssemblyName(args.Name).Name;
            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string assemblyPath = Path.Combine(folderPath, fileName + ".dll");
            FileInfo file = new FileInfo(assemblyPath);
            Assembly ass = null;
            if (!file.Exists)
            {
                string newFileName = file.Directory + "\\Modules\\" + file.Name;
                if (File.Exists(newFileName))
                {
                    ass = Assembly.LoadFile(newFileName);
                    return ass;
                }
            }
            else
            {
                ass = Assembly.LoadFile(file.FullName);
                return ass;
            }
            return ass;
        }
    }
}
