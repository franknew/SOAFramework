using SOAFramework.Server.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace SOAFramework.Server.UI
{
    static class Program
    {
        private static string moduleDir = "Modules";
        private static string dllCacheDir = "DllCache";
        private static AppDomain domain;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuleDir"]))
            {
                moduleDir = ConfigurationManager.AppSettings["ModuleDir"];
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DllCacheDir"]))
            {
                dllCacheDir = ConfigurationManager.AppSettings["DllCacheDir"];
            }
            AppDomain.CurrentDomain.SetShadowCopyFiles();
            AppDomain.CurrentDomain.SetShadowCopyPath(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + dllCacheDir);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerUI server = new ServerUI();
            domain = server.domain;
            Application.Run(server);
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
                string newFileName = file.Directory + "\\" + moduleDir + "\\" + file.Name;
                if (File.Exists(newFileName))
                {
                    ass = Assembly.LoadFile(newFileName);
                }
            }
            else
            {
                ass = Assembly.LoadFile(file.FullName);
            }
            //domain.Load(file.FullName);
            return ass;
        }
    }
}
