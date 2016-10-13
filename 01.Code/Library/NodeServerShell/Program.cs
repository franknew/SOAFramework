﻿using System;
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
            //AppDomain.CurrentDomain.SetShadowCopyFiles();
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            #region 分析参数
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            //argDic["h"] = "http://10.1.50.195/api/";
            //argDic["c"] = @"E:\AppLib\SOAFramework\01.Code\Bin\Data";
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
            var commondllfiles = common.GetFiles("*.dll", SearchOption.AllDirectories);
            DirectoryInfo apidirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            foreach (var f in commondllfiles)
            {
                string destFile = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), f.Name);
                if (!File.Exists(destFile)) f.CopyTo(destFile);
                else
                {
                    FileInfo destVersion = new FileInfo(destFile);
                    if (f.LastWriteTime > destVersion.LastWriteTime) f.CopyTo(destFile, true);
                }
            }

            var apidllfiles = apidirectory.GetFiles("*.dll", SearchOption.AllDirectories);
            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(commondllfiles);
            files.AddRange(apidllfiles);

            foreach (var f in files)
            {
                Assembly.LoadFile(f.FullName);
            }

            #endregion

            #region 启动服务
            var serverAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.GetName().Name.Equals("MicroService.Library.Server"));
            //var serverAss  = Assembly.LoadFrom(argDic["c"] + "\\MicroService.Library.Server.dll");
            dynamic server = serverAss.CreateInstance("MicroService.Library.NodeServer");
            server.CommonDllPath = _commonDirectory;
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
            string fileName = null;
            if (args.Name.Contains("\\"))
            {
                fileName = args.Name.Replace("\\\\", "\\");
            }
            else
            {
                AssemblyName name = new AssemblyName(args.Name);
                DirectoryInfo common = new DirectoryInfo(_commonDirectory);
                var file = common.GetFiles("*.dll").FirstOrDefault(t => t.Name.Equals(name.Name));
                if (file != null) fileName = file.FullName;
            }
            if (!string.IsNullOrEmpty(fileName)) a = Assembly.LoadFrom(fileName);
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
