﻿using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOAFramework.Service.UI
{
    public partial class BaseUI : Form
    {
        private List<CacheMessage> messageTemp = new List<CacheMessage>();
        public BaseUI()
        {
            InitializeComponent();
            niIcon.ContextMenu = new ContextMenu();
            niIcon.ContextMenu.MenuItems.Add(new MenuItem("显示界面", show_click));
            niIcon.ContextMenu.MenuItems.Add(new MenuItem("退出", close_Click));
        }

        private bool started = false;

        private BackgroundWorker worker = new BackgroundWorker();

        private void tbClear_Click(object sender, EventArgs e)
        {
            txbMessage.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //处理日志信息
            List<CacheMessage> list = MonitorCache.GetInstance().PopMessages(CacheEnum.FormMonitor);
            while (list.Count > 0)
            {
                if (txbMessage.Text.Length > 1024 * 1024)
                {
                    txbMessage.Clear();
                }
                else
                {
                    CacheMessage message = list[0];
                    string text = string.Format("{0} -- {1}\r\n", message.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), message.Message);
                    txbMessage.AppendText(text);
                    list.Remove(message);
                }
            }
        }

        private void BaseUI_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void niIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.Visible && this.Focused)
            {
                this.Hide();
            }
            else
            {
                this.Show();
                this.Activate();
            }
        }

        private void show_click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbStart_Click(object sender, EventArgs e)
        {
            started = true;
        }

        private void tbStop_Click(object sender, EventArgs e)
        {
            started = false;
        }

        private void BaseUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (started)
            {
                if (MessageBox.Show("服务已启动，退出会造成服务不可用。确定退出程序吗？", "退出", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}