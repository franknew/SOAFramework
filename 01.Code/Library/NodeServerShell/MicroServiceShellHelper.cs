using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroService.Library.MicroService
{
    public class ShellHelper
    {
        private static SimpleLogger _logger = new SimpleLogger();
        static ShellHelper()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly ass = null;
            
            if (args.Name.Contains("\\"))//说明是文件路径
            {
                ass = LoadAssemblyCopy(args.Name);
            }
            else//说明是dll命名
            {
                ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.FullName.Equals(args.Name));
                if (ass == null)
                {
                    var names = args.Name.Split(',');
                    var assName = names[0];
                    var fileName = string.Format("{0}{1}.dll", AppDomain.CurrentDomain.BaseDirectory, assName);
                    ass = LoadAssemblyCopy(fileName);
                }
            }
            if (ass != null) Log(string.Format("resolve dll:{0}", ass.FullName));
            else Log(string.Format("file not found:{0}", ass.FullName));
            return ass;
        }

        public static void StartInstance(string host, string commondllPath, string apidllPath)
        {
            Log("loading common dll");
            LoadDlls(commondllPath);
            Log("common dll loaded");
            Log("loading api dll");
            var list = LoadDlls(apidllPath);
            Log("api dll loaded");
            Log("starting node");
            StartService(host, list);
            Log("node started");
        }

        private static void StartService(string host, List<Assembly> list)
        {
            var serverAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.GetName().Name.Equals("MicroService.Library.Server"));
            dynamic server = serverAss.CreateInstance("MicroService.Library.NodeServer");
            //NodeServer server = new NodeServer();
            //server.Start(host, AppDomain.CurrentDomain, list);
            server.Start(host, AppDomain.CurrentDomain, list);
        }

        private static List<Assembly> LoadDlls(string dllPath)
        {
            List<Assembly> list = new List<Assembly>();
            DirectoryInfo directory = new DirectoryInfo(dllPath);
            if (!directory.Exists) directory.Create();
            var files = directory.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var ass = LoadAssemblyCopy(f.FullName);
                Log(string.Format("loaded dll:{0}", f.FullName));
                if (ass != null) list.Add(ass);
            }
            return list;
        }

        private static Assembly LoadAssemblyCopy(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            Assembly ass = null;
            if (!file.Exists) throw new Exception(string.Format("文件:{0}不存在", filePath));
            byte[] fileByte = File.ReadAllBytes(filePath);
            ass = Assembly.Load(fileByte);
            return ass;
        }

        public static void Log(string log)
        {
            Console.WriteLine(log);
            _logger.Write(log);
        }
    }
}
