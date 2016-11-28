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
            try
            {
                string packageName = txbName.Text;
                if (!string.IsNullOrEmpty(txbFile.Text))
                {
                    DirectoryInfo directory = new DirectoryInfo(txbFile.Text);
                    string destPath = Path.Combine(_server.ApiDirectory, packageName);
                    if (directory.Exists) directory.Copy(destPath);
                }
                if (_server == null) _server = new MasterServer();

                _server.StartNode(packageName);
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
                if (_server == null) _server = new MasterServer();
                txbUrl.Text = _server.Host;
                txbEntry.Text = _server.ServiceEntry;
                txbApiDirectory.Text = _server.ApiDirectory;
                txbCommonDirectory.Text = _server.CommonDirectory;
                if (autoStart == "1") button1_Click(null, null);
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
                GetPackagesResponse data = null;
                try
                {
                    data = JsonHelper.Deserialize<GetPackagesResponse>(json);
                    if (data.Result == null)
                    {
                        MessageBox.Show(json);
                        return;
                    };
                }
                catch (Exception ex)
                {

                }
                
                BindingList<NodeServerDataModel> source = dgvlist.DataSource as BindingList<NodeServerDataModel>;
                if (source == null)
                {
                    source = new BindingList<NodeServerDataModel>();
                    dgvlist.DataSource = source;
                }
                foreach (var d in data.Result)
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

                _server.Start();
                Query();
                timerTick.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
