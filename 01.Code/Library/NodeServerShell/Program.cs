using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroService.Library
{
    class Program
    {
        private static string _commonDirectory;

        static void Main(string[] args)
        {
#if DEBUG
            //Console.ReadLine();
#endif
            AppDomain.CurrentDomain.SetShadowCopyFiles();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            #region 分析参数
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            argDic["h"] = "http://10.1.50.195/api/";
            argDic["c"] = @"E:\AppLib\SOAFramework\01.Code\Bin\Data";
            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith("-")) continue;
                string arg = i < args.Length ? args[i + 1] : null;
                argDic[args[i].Trim().TrimStart('-')] = arg;
            }
            if (!argDic.ContainsKey("h")) throw new Exception("没有host(-h)参数");
            if (!argDic.ContainsKey("c")) throw new Exception("没有common dll(-c)参数");
            AppDomain.CurrentDomain.AppendPrivatePath(argDic["c"]);
            #endregion

            #region 加载公共dll
            _commonDirectory = argDic["c"];
            DirectoryInfo common = new DirectoryInfo(argDic["c"]);
            if (!common.Exists) common.Create();
            var files = common.GetFiles();
            foreach (var f in files)
            {
                var ass = Assembly.LoadFrom(f.FullName);
            }
            #endregion

            #region 启动服务
            var serverAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.GetName().Name.Equals("MicroService.Library.Server"));
            //var serverAss  = Assembly.LoadFrom(argDic["c"] + "\\MicroService.Library.Server.dll");
            dynamic server = serverAss.CreateInstance("MicroService.Library.NodeServer");
            //dynamic server = Activator.CreateInstance(serverType);
            server.Start(argDic["h"]);
            //NodeServer server = new NodeServer(argDic["h"]);
            //server.Start();
            #endregion
            Console.ReadLine();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly a = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.Equals(args.RequestingAssembly));
            if (a != null) return a;
            AssemblyName name = new AssemblyName(args.Name);
            DirectoryInfo common = new DirectoryInfo(_commonDirectory);
            var file = common.GetFiles().FirstOrDefault(t => t.Name.Equals(name.Name));
            if (file != null) a = Assembly.Load(CopyAssembly(file.FullName));
            if (a != null) return a;
            return null;
        }

        public static void CopyLoad(string fullFileName)
        {
            //FileVersionInfo file = FileVersionInfo.GetVersionInfo(fullFileName);
            //string version = file.ProductVersion;
            //string culture = "neutral";
            //string token = 
            AppDomain.CurrentDomain.Load(CopyAssembly(fullFileName));
        }

        public static byte[] CopyAssembly(string fullFileName)
        {
            byte[] assbyte = null;
            using (FileStream stream = new FileStream(fullFileName, FileMode.Open))
            {
                assbyte = new byte[stream.Length];
                stream.Read(assbyte, 0, (int)stream.Length);
            }
            return assbyte;
        }
    }
}
