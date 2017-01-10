﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;
using SOAFramework.Library;

namespace MicroService.Library
{
    class Program
    {
        private static string _commonDirectory;
        private static SimpleLogger _logger = new SimpleLogger();

        static void Main(string[] args)
        {

            //AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
            //AppDomain.CurrentDomain.SetupInformation.CachePath = "__cache";
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            try
            {
                var enableReadLine = ConfigurationManager.AppSettings["EnableReadLine"];
                if (enableReadLine == "1")
                {
                    Console.WriteLine("please enter to continue");
                    Console.ReadLine();
                }

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
                if (!argDic.ContainsKey("t")) throw new Exception("没有timing(-t)参数");
                //AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = argDic["c"];
                #endregion

                #region 加载公共dll
                _commonDirectory = argDic["c"];
                DirectoryInfo common = new DirectoryInfo(argDic["c"]);
                if (!common.Exists) common.Create();
                //AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories = AppDomain.CurrentDomain.BaseDirectory;
                //AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = common.FullName;
                var commondllfiles = common.GetFiles("*.dll", SearchOption.AllDirectories);
                DirectoryInfo apidirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                var apidllfiles = apidirectory.GetFiles("*.dll", SearchOption.AllDirectories);
                List<FileInfo> files = new List<FileInfo>();
                //files.AddRange(commondllfiles);
                files.AddRange(apidllfiles);

                // files = files.GroupBy(f => f.Name).Select(g => g.First()).ToList();

                foreach (var f in files)
                {
                    Assembly.LoadFile(f.FullName);
                }

                #endregion

                #region 启动服务
                var serverAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.GetName().Name.Equals("MicroService.Library.Server"));
                //var serverAss  = Assembly.LoadFrom(argDic["c"] + "\\MicroService.Library.Server.dll");
                dynamic server = serverAss.CreateInstance("MicroService.Library.NodeServer");
                NodeServer servers = server as NodeServer;
                servers.CommonDllPath = _commonDirectory;
                //dynamic server = Activator.CreateInstance(serverType);
                servers.Start(argDic["h"], argDic["t"] == Enum.GetName(typeof(ServerType), ServerType.Server) ? ServerType.Server : ServerType.Timing);
                //NodeServer server = new NodeServer(argDic["h"]);
                //server.Start();
                #endregion

                Console.ReadLine();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                    _logger.Write(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                var innerex = ex.InnerException;
                StringBuilder exbuilder = new StringBuilder();
                exbuilder.AppendFormat("Message:{0} -- Stack:{1}", ex.Message, ex.StackTrace);
                exbuilder.AppendLine();
                while (innerex != null)
                {
                    exbuilder.AppendFormat("Message:{0} -- Stack:{1}", innerex.Message, innerex.StackTrace);
                    exbuilder.AppendLine();
                    innerex = innerex.InnerException;
                }
                _logger.Write(exbuilder.ToString());
            }
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
                var file = common.GetFiles("*.dll").FirstOrDefault(t => t.Name.Equals(name.Name + ".dll"));
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
