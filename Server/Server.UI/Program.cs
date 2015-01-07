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
        private static AppDomain serviceDomain;
        private static string moduleDir = "Modules";
        private static string dllCacheDir = "DllCache";
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
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
            string cacheDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + dllCacheDir;
            AppDomain.CurrentDomain.SetupInformation.CachePath = cacheDir;
            AppDomain.CurrentDomain.SetShadowCopyFiles();
            AppDomain.CurrentDomain.SetCachePath(cacheDir);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerUI form = new ServerUI();
            serviceDomain = form.ServiceDomain;
            Application.Run(form);
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
                    byte[] bytAss = File.ReadAllBytes(newFileName);
                    ass = Assembly.Load(bytAss);
                    return ass;
                }
            }
            else
            {
                byte[] bytAss = File.ReadAllBytes(file.FullName);
                ass = Assembly.Load(bytAss);
                return ass;
            }
            serviceDomain.Load(ass.Location);
            return null;
        }
        
    }
}
