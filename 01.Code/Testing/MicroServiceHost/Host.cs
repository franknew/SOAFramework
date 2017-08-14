﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using MicroService.Library;
using SOAFramework.Library;
using System.IO;
using System.Threading.Tasks;

namespace MicroServiceTesting
{
    public partial class Host : Form
    {
        public Host()
        {
            InitializeComponent();
        }

        private MasterServer _server;

        private void button1_Click(object sender, EventArgs e)
        {
            Deploy(_server.ApiDirectory, ServerType.Server);
        }

        private void Deploy(string targetDirectory, ServerType serverType)
        {
            try
            {
                string packageName = txbName.Text;
                if (!string.IsNullOrEmpty(txbFile.Text))
                {
                    DirectoryInfo directory = new DirectoryInfo(txbFile.Text);
                    string destPath = Path.Combine(targetDirectory, packageName);
                    if (directory.Exists)
                    {
                        directory.Copy(destPath);
                    }

                    if (!Directory.Exists(_server.CommonDirectory))
                    {
                        Directory.CreateDirectory(_server.CommonDirectory);
                    }

                    string sourcePath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
                    string serviceSourceEntryPath = Path.Combine(sourcePath, _server.ServiceEntry);
                    string serviceTargetEntryPath = Path.Combine(_server.CommonDirectory, _server.ServiceEntry);


                    if (File.Exists(serviceSourceEntryPath))
                    {
                        File.Copy(serviceSourceEntryPath, serviceTargetEntryPath, true);
                    }

                    string[] modules = (ConfigurationManager.AppSettings["Moudles"] + "").Split(',');

                    foreach (var modul in modules)
                    {
                        if (string.IsNullOrEmpty(modul))
                        {
                            continue;
                        }
                        string fileName = Path.Combine(sourcePath, string.Format("Modules\\{0}", modul));
                        if (File.Exists(fileName))
                        {
                            File.Copy(fileName, Path.Combine(destPath, modul), true);
                        }
                    }

                }
                if (_server == null) _server = new MasterServer();

                _server.StartNode(packageName, serverType);
                MessageBox.Show("部署并启动成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void timerTick_Tick(object sender, EventArgs e)
        {
            Query();
        }

        private void Host_Load(object sender, EventArgs e)
        {
            try
            {
                string autoStart = ConfigurationManager.AppSettings["AutoStart"];
                string displayShell = ConfigurationManager.AppSettings["DisplayShell"];
                string autoStartTiming = ConfigurationManager.AppSettings["AutoStartTiming"];
                if (_server == null) _server = new MasterServer();
                _server.DisplayShell = displayShell == "1";
                txbUrl.Text = _server.Host;
                txbEntry.Text = _server.ServiceEntry;
                txbApiDirectory.Text = _server.ApiDirectory;
                txbCommonDirectory.Text = _server.CommonDirectory;
                txtTimingDirectory.Text = _server.TimingDirectory;
                if (autoStart == "1") btnServer_Click(null, null);
                //if (autoStartTiming == "1") btnStartTiming_Click(null, null);
                if (chkAutoRefresh.Checked)
                {
                    timerTask.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Query()
        {
            try
            {
                string host = ConfigurationManager.AppSettings[Config.Host];
                string url = string.Format("{0}/{1}/MicroService.Library.DiscoveryService.GetPackages", host.TrimEnd('/'), Config.Server);

                string json = HttpHelper.Get(url);
                BindingList<NodeServerDataModel> data = null;
                try
                {
                    data = JsonHelper.Deserialize<BindingList<NodeServerDataModel>>(json);
                    if (data == null)
                    {
                        MessageBox.Show(json);
                        return;
                    };
                }
                catch (Exception ex)
                {

                }

                BindingList<NodeServerDataModel> source = dgvlist.DataSource as BindingList<NodeServerDataModel>;
                if (source == null || data.Count == 0)
                {
                    source = new BindingList<NodeServerDataModel>();
                    dgvlist.DataSource = source;
                }
                foreach (var d in data)
                {
                    var p = source.FirstOrDefault(t => t.PackageName.Equals(d.PackageName));
                    if (p == null) source.Add(d);
                    else
                    {
                        p.Status = d.Status;
                        p.Error = d.Error;
                        p.Url = d.Url;
                    }
                }
                dgvlist.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 查询任务集合
        /// </summary>
        private void QueryTasks()
        {
            BindingList<TimingTaskInfo> tasks = new BindingList<TimingTaskInfo>();
            var timings = GetTimings();

            if (timings == null)
            {
                return;
            }

            foreach (var item in timings)
            {
                if (item.Status != ServerStatusType.Started && item.Status != ServerStatusType.Wait)
                {
                    continue;
                }

                string url = string.Format("{0}{1}", item.Url, "MicroService.Library.TimingTasksManager.Query");

                try
                {
                    string json = HttpHelper.Get(url);
                    List<TimingTaskInfo> data = JsonHelper.Deserialize<List<TimingTaskInfo>>(json);
                    if (data == null)
                    {
                        MessageBox.Show(json);
                        return;
                    };
                    foreach (var task in data)
                    {
                        task.TaskUrl = item.Url;
                        tasks.Add(task);
                    }
                }
                catch (Exception)
                {

                }
            }
            var source = dgvTasks.DataSource as BindingList<TimingTaskInfo>;
            if (source == null)
            {
                dgvTasks.AutoGenerateColumns = false;
                dgvTasks.DataSource = tasks;
            }
            else
            {
                foreach (var t in tasks)
                {
                    var task = source.FirstOrDefault(s => s.Key == t.Key);
                    if (task != null)
                    {
                        task.SetState(t.State);
                        task.StartExecuteTime = t.StartExecuteTime;
                        task.ExecuteCount = t.ExecuteCount;
                    }
                    else
                    {
                        source.Add(t);
                    }
                }

                for (int i = 0; i < source.Count; i++)
                {
                    if (tasks.Where(t => t.Key == source[i].Key).Count() == 0)
                    {
                        source.Remove(source[i]);
                        i--;
                    }
                }

            }
            dgvTasks.Refresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (timerTick.Enabled)
            {
                timerTick.Stop();
                btnStart.Text = "启动自动刷新";
            }
            else
            {
                timerTick.Start();
                btnStart.Text = "停止自动刷新";
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void Host_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_server != null) _server.CloseAllNode();
        }

        private void dgvlist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            var column = dgvlist.Columns[e.ColumnIndex];
            var row = dgvlist.Rows[e.RowIndex];
            var data = row.DataBoundItem as NodeServerDataModel;
            switch (column.Name)
            {
                case "启动":
                    _server.StartNode(data.PackageName);
                    Query();
                    break;
                case "重启":
                    _server.RestartNode(data.PackageName);
                    Query();
                    break;
                case "停止":
                    _server.CloseNode(data.PackageName);
                    Query();
                    break;
            }
        }

        private void dgvlist_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvlist.Rows[e.RowIndex];
            var data = row.DataBoundItem as NodeServerDataModel;
            switch (data.Status)
            {
                case ServerStatusType.Started:
                    row.Cells["启动"].ReadOnly = true;
                    row.Cells["停止"].ReadOnly = false;
                    break;
                case ServerStatusType.Stoped:
                case ServerStatusType.Wait:
                case ServerStatusType.Close:
                case ServerStatusType.Error:
                    row.Cells["启动"].ReadOnly = false;
                    row.Cells["停止"].ReadOnly = true;
                    break;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbFile.Text = dialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string host = txbUrl.Text;
            string api = txbApiDirectory.Text;
            string common = txbCommonDirectory.Text;
            string entry = txbEntry.Text;
            string timing = txtTimingDirectory.Text;
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[Config.Host] == null) config.AppSettings.Settings.Add(Config.Host, host);
            else config.AppSettings.Settings[Config.Host].Value = host;
            if (config.AppSettings.Settings[Config.ApiDirectory] == null) config.AppSettings.Settings.Add(Config.ApiDirectory, api);
            else config.AppSettings.Settings[Config.ApiDirectory].Value = api;
            if (config.AppSettings.Settings[Config.CommonDirectory] == null) config.AppSettings.Settings.Add(Config.CommonDirectory, common);
            else config.AppSettings.Settings[Config.CommonDirectory].Value = common;
            if (config.AppSettings.Settings[Config.ServiceEntry] == null) config.AppSettings.Settings.Add(Config.ServiceEntry, entry);
            else config.AppSettings.Settings[Config.ServiceEntry].Value = entry;
            if (config.AppSettings.Settings[Config.TimingDirectory] == null) config.AppSettings.Settings.Add(Config.TimingDirectory, timing);
            else config.AppSettings.Settings[Config.TimingDirectory].Value = timing;
            config.Save(ConfigurationSaveMode.Modified);

            System.Configuration.ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存配置成功！");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //NodeServer server = new NodeServer();
            //server.CommonDllPath = @"E:\AppLib\SOAFramework\01.Code\Bin\MicroServiceTesting\Common";
            //server.Start("http://10.1.50.195/面单接口/");
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    _server.Start();
                });
                Task.WaitAll(task);
                Query();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStartAllTask_Click(object sender, EventArgs e)
        {
            foreach (var item in (BindingList<NodeServerDataModel>)dgvlist.DataSource)
            {
                try
                {
                    string url = string.Format("{0}{1}", item.Url, "MicroService.Library.TimingTasksManager.StartAllTask");
                    HttpHelper.Get(url);
                }
                catch
                {

                }
            }
            QueryTasks();
        }

        private void btnStopAllTask_Click(object sender, EventArgs e)
        {
            foreach (var item in (BindingList<NodeServerDataModel>)dgvlist.DataSource)
            {
                try
                {
                    string url = string.Format("{0}{1}", item.Url, "MicroService.Library.TimingTasksManager.StopAllTask");
                    HttpHelper.Get(url);
                }
                catch
                {

                }
            }
            QueryTasks();
        }

        private void btnRefreshTask_Click(object sender, EventArgs e)
        {
            QueryTasks();
        }



        private void tabPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPages.SelectedIndex == 3)
            {
                QueryTasks();
            }
        }

        private void timerTask_Tick(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked && tabPages.SelectedIndex == 3)
            {
                QueryTasks();
            }
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked)
            {
                timerTask.Start();
            }
            else
            {
                timerTask.Stop();
            }

        }

        private void dgvTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                return;
            }

            var column = dgvTasks.Columns[e.ColumnIndex];
            string url = string.Empty;
            switch (column.HeaderText)
            {
                case "启动":
                    url = string.Format("{0}{1}?taskKey={2}", dgvTasks.Rows[e.RowIndex].Cells["colTaskUrl"].Value, "MicroService.Library.TimingTasksManager.StartTask", dgvTasks.Rows[e.RowIndex].Cells["colTaskKey"].Value);
                    HttpHelper.Get(url);
                    QueryTasks();
                    break;
                case "停止":
                    url = string.Format("{0}{1}?taskKey={2}", dgvTasks.Rows[e.RowIndex].Cells["colTaskUrl"].Value, "MicroService.Library.TimingTasksManager.StopTask", dgvTasks.Rows[e.RowIndex].Cells["colTaskKey"].Value);
                    HttpHelper.Get(url);
                    QueryTasks();
                    break;
                case "打开":
                    var address = Convert.ToString(dgvTasks.Rows[e.RowIndex].Cells["colTaskAddress"].Value);
                    if (!string.IsNullOrEmpty(address))
                    {
                        var fileAddress = address.Split('【');
                        if (fileAddress.Count() > 0 && File.Exists(fileAddress[0].Trim()))
                        {
                            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                            psi.Arguments = "/e,/select," + fileAddress[0].Trim();
                            System.Diagnostics.Process.Start(psi);
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// 获取定时器
        /// </summary>
        /// <returns></returns>
        private List<NodeServerDataModel> GetTimings()
        {
            List<NodeServerDataModel> data = null;
            try
            {
                string host = ConfigurationManager.AppSettings[Config.Host];
                string url = string.Format("{0}/{1}/MicroService.Library.DiscoveryService.GetTimings", host.TrimEnd('/'), Config.Server);

                string json = HttpHelper.Get(url);


                data = JsonHelper.Deserialize<List<NodeServerDataModel>>(json);
                if (data == null)
                {
                    MessageBox.Show(json);

                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return data;
        }

        private void btnPupTiming_Click(object sender, EventArgs e)
        {
            Deploy(_server.TimingDirectory, ServerType.Timing);
        }


        private void btnStartTiming_Click(object sender, EventArgs e)
        {
            _server.StartAllTiming();

            QueryTasks();
        }

        private void btnStopTiming_Click(object sender, EventArgs e)
        {
            _server.CloseAllTiming();
            QueryTasks();
        }
    }
}
