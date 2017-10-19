using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Security.Policy;

namespace MicroService.Library.MicroService
{
    [Serializable]
    class Program
    {
        const string domainName = "__NodeServer";
        private static string _host;
        private static string _commonDirectory;

        static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
                var enableReadLine = ConfigurationManager.AppSettings["EnableReadLine"];
                var disabled = ConfigurationManager.AppSettings["Disabled"];
                if (disabled == "1") return;
                if (enableReadLine == "1")
                {
                    Console.WriteLine("please enter to continue");
                    Console.ReadLine();
                }

                #region 分析参数
                Dictionary<string, string> argDic = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++)
                {
                    if (!args[i].StartsWith("-")) continue;
                    string arg = i < args.Length ? args[i + 1] : null;
                    argDic[args[i].Trim().TrimStart('-')] = arg;
                }
                if (!argDic.ContainsKey("h")) throw new Exception("没有host(-h)参数");
                if (!argDic.ContainsKey("c")) throw new Exception("没有common dll(-c)参数");
                //if (!argDic.ContainsKey("t")) throw new Exception("没有timing(-t)参数");
                //_serverType = argDic["t"];
                _host = argDic["h"];
                _commonDirectory = argDic["c"];
                var _apiDirectory = AppDomain.CurrentDomain.BaseDirectory;
                #endregion

                #region 启动服务
                ShellHelper.StartInstance(_host, _commonDirectory, _apiDirectory);
                #endregion

                #region 监视文件更新，热更新
                //FileSystemWatcher fileWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                //fileWatcher.Changed += FileWatcher_Changed;
                //fileWatcher.Created += FileWatcher_Changed;
                //fileWatcher.Deleted += FileWatcher_Changed;
                //fileWatcher.WaitForChanged(WatcherChangeTypes.All);
                //Console.WriteLine("文件已加入监控...");
                #endregion

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
                    Console.WriteLine(sb.ToString());
                    ShellHelper.Log(sb.ToString());
                    //_logger.Write(sb.ToString());
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
                Console.WriteLine(innerex.ToString());
                ShellHelper.Log(innerex.ToString());
                //_logger.Write(exbuilder.ToString());
                //Console.WriteLine(exbuilder.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            //Console.WriteLine(string.Format("load file:{0} name:{1}", args.LoadedAssembly.Location,  args.LoadedAssembly.FullName));
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            var asses = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in asses)
            {
                if (ass.FullName.Equals(args.Name)) return ass;
            }
            return assembly;
        }


        #region commented
        //private static AppDomain CreateShadowDomain(string commondllPath)
        //{
        //    _files.Clear();
        //    AppDomain domain = null;
        //    AppDomainSetup setupinfo = new AppDomainSetup();
        //    setupinfo.ApplicationName = "api";
        //    setupinfo.ShadowCopyFiles = "true";
        //    setupinfo.CachePath = AppDomain.CurrentDomain.BaseDirectory + "_cache";
        //    setupinfo.DynamicBase = setupinfo.PrivateBinPath = commondllPath;
        //    setupinfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;
        //    setupinfo.ShadowCopyDirectories = setupinfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

        //    domain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, setupinfo);
        //    domain.DoCallBack(new CrossAppDomainDelegate(DoCallBack));
        //    //domain.AssemblyResolve += Domain_AssemblyResolve;
        //    DirectoryInfo common = new DirectoryInfo(commondllPath);
        //    if (!common.Exists) common.Create();
        //    var commondllfiles = common.GetFiles("*.dll", SearchOption.AllDirectories);

        //    DirectoryInfo apidirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        //    var apidllfiles = apidirectory.GetFiles("*.dll", SearchOption.AllDirectories);
        //    //files.AddRange(commondllfiles);
        //    _files.AddRange(apidllfiles);
        //    string assemblyName = Assembly.GetExecutingAssembly().GetName().FullName;

        //    //TransparentAgent factory = (TransparentAgent)_apiDomain.CreateInstanceAndUnwrap(assemblyName, typeof(TransparentAgent).FullName);
        //    // files = files.GroupBy(f => f.Name).Select(g => g.First()).ToList();
        //    foreach (var f in _files)
        //    {
        //        Console.WriteLine(f.FullName);
        //        domain.Load(f.FullName);
        //        //Assembly.LoadFile(f.FullName);
        //    }
        //    return domain;
        //}
        #endregion

    }
}
