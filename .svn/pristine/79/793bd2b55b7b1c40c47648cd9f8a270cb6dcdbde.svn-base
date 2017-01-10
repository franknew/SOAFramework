using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AustinHarris.JsonRpc;
using System.Configuration;
using SOAFramework.Library;
using System.Runtime;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace MicroService.Library
{
    public class MasterServer : IDisposable
    {
        private static readonly Dictionary<string, NodeServerDataModel> _packages = new Dictionary<string, NodeServerDataModel>();
        private static Dictionary<string, Process> _processes = new Dictionary<string, Process>();
        private string _host = null;
        private string _apiDirectory = null;
        private string _commonDirectory = null;
        private string _serviceEntry = null;
        private string _timingDirectory = null;

        private NodeServer _server = null;


        public string Host
        {
            get { return _host; }
        }
        public string ApiDirectory
        {
            get { return _apiDirectory; }
        }
        public string CommonDirectory
        {
            get { return _commonDirectory; }
        }
        public string ServiceEntry
        {
            get { return _serviceEntry; }
        }

        public string TimingDirectory
        {
            get { return _timingDirectory; }
        }

        public bool DisplayShell { get; set; }

        public static List<NodeServerDataModel> GetPackages()
        {
            List<NodeServerDataModel> list = new List<NodeServerDataModel>();
            foreach (var key in _packages.Keys)
            {
                if (_processes[key].HasExited) _packages[key].Status = ServerStatusType.Close;
                else _packages[key].Status = ServerStatusType.Started;
                if (_packages[key].ServerType == ServerType.Server)
                {
                    list.Add(_packages[key]);
                }

            }
            return list;
        }

        /// <summary>
        /// 获取所有定时任务
        /// </summary>
        /// <returns></returns>
        public static List<NodeServerDataModel> GetTimings()
        {
            List<NodeServerDataModel> list = new List<NodeServerDataModel>();
            foreach (var key in _packages.Keys)
            {
                if (_processes[key].HasExited) _packages[key].Status = ServerStatusType.Close;
                else _packages[key].Status = ServerStatusType.Started;
                if (_packages[key].ServerType == ServerType.Timing)
                {
                    list.Add(_packages[key]);
                }

            }
            return list;
        }

        public MasterServer()
        {
            AppDomain.CurrentDomain.SetShadowCopyFiles();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Config.Host])) _host = ConfigurationManager.AppSettings[Config.Host];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Config.ApiDirectory])) _apiDirectory = ConfigurationManager.AppSettings[Config.ApiDirectory];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Config.CommonDirectory])) _commonDirectory = ConfigurationManager.AppSettings[Config.CommonDirectory];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Config.ServiceEntry])) _serviceEntry = ConfigurationManager.AppSettings[Config.ServiceEntry];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Config.TimingDirectory])) _timingDirectory = ConfigurationManager.AppSettings[Config.TimingDirectory];

            if (string.IsNullOrEmpty(_host)) throw new Exception("没有host");
            if (string.IsNullOrEmpty(_apiDirectory)) throw new Exception("没有ApiDirectory");
            if (string.IsNullOrEmpty(_commonDirectory)) throw new Exception("没有CommonDirectory");
            if (string.IsNullOrEmpty(_serviceEntry)) throw new Exception("没有ServiceEntry");
            if (string.IsNullOrEmpty(_timingDirectory)) throw new Exception("没有TimingDirectory");
        }

        public void Start()
        {
            StartAllNode();

            string serverurl = string.Format("{0}/{1}/", _host.TrimEnd('/'), Config.Server);
            if (_server == null || _server.Status == ServerStatus.Close) _server = new NodeServer(url: serverurl);
            if (_server.Status == ServerStatus.Start) _server.Stop();
            _server.Start();
        }

        public void StartNode(string packageName, ServerType serverType = ServerType.Server)
        {
            //如果进程没有关闭，就强行关闭
            string processName = string.Format("{0}.{1}", Config.MicroService, packageName);
            string destPath = string.Format(@"{0}\{1}\", serverType == ServerType.Server ? _apiDirectory.TrimEnd('\\') : _timingDirectory.TrimEnd('\\'), packageName).TrimEnd('\\');
            string entrydestfile = string.Format(@"{0}\{1}.exe", destPath.TrimEnd('\\'), processName);
            string entry = string.Format(@"{0}\{1}", _commonDirectory, _serviceEntry);
            string url = string.Format("{0}/{1}/", _host.TrimEnd('/'), packageName);
            NodeServerDataModel node = null;

            try
            {
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
                if (!File.Exists(entrydestfile)) File.Copy(entry, entrydestfile);

                #region 复制dll
                DirectoryInfo common = new DirectoryInfo(_commonDirectory);
                if (!common.Exists) common.Create();
                var commondllfiles = common.GetFiles("*.dll", SearchOption.AllDirectories);
                DirectoryInfo apidirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                foreach (var f in commondllfiles)
                {
                    string destFile = string.Format("{0}\\{1}", destPath, f.Name);
                    if (!File.Exists(destFile)) f.CopyTo(destFile);
                    else
                    {
                        FileInfo destVersion = new FileInfo(destFile);
                        if (f.LastWriteTime > destVersion.LastWriteTime) f.CopyTo(destFile, true);
                    }
                }
                #endregion

                Process p = Process.GetProcessesByName(processName).FirstOrDefault();
                if (p == null)
                {
                    p = new Process();
                    ProcessStartInfo ps = new ProcessStartInfo();
                    //ps.RedirectStandardError = true;
                    //ps.RedirectStandardOutput = true;
                    ps.CreateNoWindow = !DisplayShell;
                    ps.FileName = entrydestfile;
                    ps.UseShellExecute = false;
                    if (DisplayShell) ps.WindowStyle = ProcessWindowStyle.Normal;
                    else ps.WindowStyle = ProcessWindowStyle.Hidden;
                    string args = string.Format("-h {0} -c {1} -t {2}", url, _commonDirectory, Enum.GetName(typeof(ServerType), serverType));
                    ps.Arguments = args;
                    p.ErrorDataReceived += P_ErrorDataReceived;
                    p.Exited += P_Exited;
                    p.StartInfo = ps;
                    p.EnableRaisingEvents = true;
                    p.Start();
                }
                //p.BeginErrorReadLine();
                node = SetData(packageName, ServerStatusType.Started, null, url, serverType);

                _processes[packageName] = p;
            }
            catch (Exception ex)
            {
                node = SetData(packageName, ServerStatusType.Error, ex.Message, url, serverType);
            }

            _packages[packageName] = node;


        }

        public void StartAllNode()
        {
            DirectoryInfo api = new DirectoryInfo(_apiDirectory);
            if (!api.Exists) api.Create();

            var subdirectory = api.GetDirectories();
            string entry = string.Format("{0}\\{1}", _commonDirectory, _serviceEntry);
            if (!File.Exists(entry)) throw new Exception(string.Format("目录：{0}，找不到微服务启动程序:{1}", _commonDirectory, _serviceEntry));

            foreach (var d in subdirectory)
            {
                StartNode(d.Name);
            }
        }

        /// <summary>
        /// 启动所有定时器
        /// </summary>
        public void StartAllTiming()
        {
            DirectoryInfo timing = new DirectoryInfo(_timingDirectory);
            if (!timing.Exists) timing.Create();

            var subdirectory = timing.GetDirectories();
            string entry = string.Format("{0}\\{1}", _commonDirectory, _serviceEntry);
            if (!File.Exists(entry)) throw new Exception(string.Format("目录：{0}，找不到微服务启动程序:{1}", _commonDirectory, _serviceEntry));

            foreach (var d in subdirectory)
            {
                StartNode(d.Name, ServerType.Timing);
            }
        }

        /// <summary>
        /// 关闭所有定时器
        /// </summary>
        public void CloseAllTiming()
        {
            DirectoryInfo timing = new DirectoryInfo(_timingDirectory);
            var subdirectory = timing.GetDirectories();
            foreach (var s in subdirectory)
            {
                CloseNode(s.Name);
            }
        }

        public void CloseNode(string packageName)
        {
            if (_processes.ContainsKey(packageName) && !_processes[packageName].HasExited) _processes[packageName].Kill();
            if (_packages.ContainsKey(packageName)) _packages[packageName].Status = ServerStatusType.Close;
        }

        public void CloseAllNode()
        {
            DirectoryInfo api = new DirectoryInfo(_apiDirectory);
            var subdirectory = api.GetDirectories();
            foreach (var s in subdirectory)
            {
                CloseNode(s.Name);
            }
        }

        public void RestartNode(string packageName)
        {
            CloseNode(packageName);
            StartNode(packageName);
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var p = sender as Process;
            foreach (var key in _processes.Keys)
            {
                if (_processes[key].Equals(p))
                {
                    _packages[key].Status = ServerStatusType.Error;
                    _packages[key].Error = e.Data;
                    break;
                }
            }
        }

        private void P_Exited(object sender, EventArgs e)
        {
            var p = sender as Process;
            foreach (var key in _processes.Keys)
            {
                if (_processes[key].Equals(p))
                {
                    _packages[key].Status = ServerStatusType.Close;
                    break;
                }
            }
        }

        private NodeServerDataModel SetData(string packageName, ServerStatusType status, string error, string url, ServerType serverType)
        {
            NodeServerDataModel node = null;
            if (_packages.ContainsKey(packageName)) node = _packages[packageName];
            else node = new NodeServerDataModel();
            node.PackageName = packageName;
            node.Status = status;
            node.Url = url;
            node.Error = error;
            node.ServerType = serverType;
            return node;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly a = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.Equals(args.RequestingAssembly));
            if (a != null) return a;
            AssemblyName name = new AssemblyName(args.Name);
            DirectoryInfo common = new DirectoryInfo(_commonDirectory);
            var file = common.GetFiles("*.dll").FirstOrDefault(t => t.Name.Equals(name.Name));
            if (file != null) a = Assembly.Load(file.FullName);
            if (a != null) return a;
            return null;
        }

        public void Dispose()
        {
            foreach (var key in _processes.Keys)
            {
                _processes[key].Kill();
                _processes[key].Dispose();
            }
        }
    }
}
