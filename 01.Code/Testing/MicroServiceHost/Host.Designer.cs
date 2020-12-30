﻿namespace MicroServiceTesting
{
    partial class Host
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Host));
            this.btnPub = new System.Windows.Forms.Button();
            this.txbName = new System.Windows.Forms.TextBox();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabList = new System.Windows.Forms.TabPage();
            this.dgvlist = new System.Windows.Forms.DataGridView();
            this.名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.启动 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.重启 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.停止 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.错误信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTimingDirectory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txbEntry = new System.Windows.Forms.TextBox();
            this.txbApiDirectory = new System.Windows.Forms.TextBox();
            this.txbCommonDirectory = new System.Windows.Forms.TextBox();
            this.txbUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPupTiming = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txbFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colTaskKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnStopTiming = new System.Windows.Forms.Button();
            this.btnStartTiming = new System.Windows.Forms.Button();
            this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
            this.btnRefreshTask = new System.Windows.Forms.Button();
            this.btnStopAllTask = new System.Windows.Forms.Button();
            this.btnStartAllTask = new System.Windows.Forms.Button();
            this.timerTick = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timerTask = new System.Windows.Forms.Timer(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.tabPages.SuspendLayout();
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvlist)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabAdd.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPub
            // 
            this.btnPub.Location = new System.Drawing.Point(10, 23);
            this.btnPub.Name = "btnPub";
            this.btnPub.Size = new System.Drawing.Size(75, 23);
            this.btnPub.TabIndex = 0;
            this.btnPub.Text = "部署并启动";
            this.btnPub.UseVisualStyleBackColor = true;
            this.btnPub.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbName
            // 
            this.txbName.Location = new System.Drawing.Point(135, 60);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(728, 21);
            this.txbName.TabIndex = 1;
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.tabList);
            this.tabPages.Controls.Add(this.tabAdd);
            this.tabPages.Controls.Add(this.tabLog);
            this.tabPages.Controls.Add(this.tabPage1);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(944, 490);
            this.tabPages.TabIndex = 2;
            this.tabPages.SelectedIndexChanged += new System.EventHandler(this.tabPages_SelectedIndexChanged);
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.dgvlist);
            this.tabList.Controls.Add(this.groupBox1);
            this.tabList.Location = new System.Drawing.Point(4, 22);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(3);
            this.tabList.Size = new System.Drawing.Size(936, 464);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "接口状态监控";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // dgvlist
            // 
            this.dgvlist.AllowUserToAddRows = false;
            this.dgvlist.AllowUserToDeleteRows = false;
            this.dgvlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvlist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.名称,
            this.URL,
            this.状态,
            this.启动,
            this.重启,
            this.停止,
            this.错误信息});
            this.dgvlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvlist.Location = new System.Drawing.Point(3, 47);
            this.dgvlist.Name = "dgvlist";
            this.dgvlist.ReadOnly = true;
            this.dgvlist.RowTemplate.Height = 23;
            this.dgvlist.Size = new System.Drawing.Size(930, 414);
            this.dgvlist.TabIndex = 1;
            this.dgvlist.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvlist_CellClick);
            this.dgvlist.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvlist_RowPrePaint);
            // 
            // 名称
            // 
            this.名称.DataPropertyName = "PackageName";
            this.名称.HeaderText = "名称";
            this.名称.Name = "名称";
            this.名称.ReadOnly = true;
            // 
            // URL
            // 
            this.URL.DataPropertyName = "Url";
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Width = 300;
            // 
            // 状态
            // 
            this.状态.DataPropertyName = "Status";
            this.状态.HeaderText = "状态";
            this.状态.Name = "状态";
            this.状态.ReadOnly = true;
            // 
            // 启动
            // 
            this.启动.HeaderText = "启动";
            this.启动.Name = "启动";
            this.启动.ReadOnly = true;
            this.启动.Text = "启动";
            this.启动.UseColumnTextForButtonValue = true;
            // 
            // 重启
            // 
            this.重启.HeaderText = "重启";
            this.重启.Name = "重启";
            this.重启.ReadOnly = true;
            this.重启.Text = "重启";
            this.重启.UseColumnTextForButtonValue = true;
            // 
            // 停止
            // 
            this.停止.HeaderText = "停止";
            this.停止.Name = "停止";
            this.停止.ReadOnly = true;
            this.停止.Text = "停止";
            this.停止.UseColumnTextForButtonValue = true;
            // 
            // 错误信息
            // 
            this.错误信息.DataPropertyName = "Error";
            this.错误信息.HeaderText = "错误信息";
            this.错误信息.Name = "错误信息";
            this.错误信息.ReadOnly = true;
            this.错误信息.Width = 300;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(930, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(7, 15);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(75, 23);
            this.btnServer.TabIndex = 2;
            this.btnServer.Text = "启动服务 ";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(169, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "启动自动刷新";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(88, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabAdd
            // 
            this.tabAdd.Controls.Add(this.groupBox3);
            this.tabAdd.Controls.Add(this.groupBox2);
            this.tabAdd.Location = new System.Drawing.Point(4, 22);
            this.tabAdd.Name = "tabAdd";
            this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdd.Size = new System.Drawing.Size(936, 464);
            this.tabAdd.TabIndex = 0;
            this.tabAdd.Text = "接口部署";
            this.tabAdd.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTimingDirectory);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.txbEntry);
            this.groupBox3.Controls.Add(this.txbApiDirectory);
            this.groupBox3.Controls.Add(this.txbCommonDirectory);
            this.groupBox3.Controls.Add(this.txbUrl);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 141);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(930, 320);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "配置";
            // 
            // txtTimingDirectory
            // 
            this.txtTimingDirectory.Location = new System.Drawing.Point(135, 145);
            this.txtTimingDirectory.Name = "txtTimingDirectory";
            this.txtTimingDirectory.Size = new System.Drawing.Size(728, 21);
            this.txtTimingDirectory.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "定时器目录";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(366, 225);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txbEntry
            // 
            this.txbEntry.Location = new System.Drawing.Point(135, 183);
            this.txbEntry.Name = "txbEntry";
            this.txbEntry.Size = new System.Drawing.Size(728, 21);
            this.txbEntry.TabIndex = 10;
            // 
            // txbApiDirectory
            // 
            this.txbApiDirectory.Location = new System.Drawing.Point(135, 108);
            this.txbApiDirectory.Name = "txbApiDirectory";
            this.txbApiDirectory.Size = new System.Drawing.Size(728, 21);
            this.txbApiDirectory.TabIndex = 9;
            // 
            // txbCommonDirectory
            // 
            this.txbCommonDirectory.Location = new System.Drawing.Point(135, 66);
            this.txbCommonDirectory.Name = "txbCommonDirectory";
            this.txbCommonDirectory.Size = new System.Drawing.Size(728, 21);
            this.txbCommonDirectory.TabIndex = 8;
            // 
            // txbUrl
            // 
            this.txbUrl.Location = new System.Drawing.Point(135, 32);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(728, 21);
            this.txbUrl.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(98, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "url";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "服务入口模板文件";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "接口目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "公共目录";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPupTiming);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txbName);
            this.groupBox2.Controls.Add(this.btnSelect);
            this.groupBox2.Controls.Add(this.btnPub);
            this.groupBox2.Controls.Add(this.txbFile);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(930, 138);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "部署";
            // 
            // btnPupTiming
            // 
            this.btnPupTiming.Location = new System.Drawing.Point(100, 23);
            this.btnPupTiming.Name = "btnPupTiming";
            this.btnPupTiming.Size = new System.Drawing.Size(102, 23);
            this.btnPupTiming.TabIndex = 7;
            this.btnPupTiming.Text = "部署监听并启动";
            this.btnPupTiming.UseVisualStyleBackColor = true;
            this.btnPupTiming.Click += new System.EventHandler(this.btnPupTiming_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(224, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "部署并启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(815, 95);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(48, 23);
            this.btnSelect.TabIndex = 5;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txbFile
            // 
            this.txbFile.Location = new System.Drawing.Point(135, 97);
            this.txbFile.Name = "txbFile";
            this.txbFile.ReadOnly = true;
            this.txbFile.Size = new System.Drawing.Size(674, 21);
            this.txbFile.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "服务包名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "源服务包文件夹";
            // 
            // tabLog
            // 
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Size = new System.Drawing.Size(936, 464);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "日志监控";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvTasks);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(936, 464);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "定时任务监控";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvTasks
            // 
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AllowUserToDeleteRows = false;
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colTaskAddress,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewButtonColumn1,
            this.dataGridViewButtonColumn2,
            this.dataGridViewButtonColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Column1,
            this.Column2,
            this.colTaskKey,
            this.colTaskUrl});
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Location = new System.Drawing.Point(3, 47);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.RowHeadersVisible = false;
            this.dgvTasks.RowTemplate.Height = 23;
            this.dgvTasks.Size = new System.Drawing.Size(930, 414);
            this.dgvTasks.TabIndex = 2;
            this.dgvTasks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTasks_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "任务名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // colTaskAddress
            // 
            this.colTaskAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTaskAddress.DataPropertyName = "TaskAddress";
            this.colTaskAddress.HeaderText = "地址";
            this.colTaskAddress.Name = "colTaskAddress";
            this.colTaskAddress.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TimeInterval";
            this.dataGridViewTextBoxColumn3.HeaderText = "执行间隔(毫秒)";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.DataPropertyName = "RunTime";
            this.dataGridViewButtonColumn1.HeaderText = "已运行时间";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.ReadOnly = true;
            this.dataGridViewButtonColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewButtonColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewButtonColumn2
            // 
            this.dataGridViewButtonColumn2.DataPropertyName = "ExecuteCount";
            this.dataGridViewButtonColumn2.HeaderText = "执行次数";
            this.dataGridViewButtonColumn2.Name = "dataGridViewButtonColumn2";
            this.dataGridViewButtonColumn2.ReadOnly = true;
            this.dataGridViewButtonColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewButtonColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewButtonColumn2.Width = 85;
            // 
            // dataGridViewButtonColumn3
            // 
            this.dataGridViewButtonColumn3.DataPropertyName = "StateName";
            this.dataGridViewButtonColumn3.HeaderText = "运行状态";
            this.dataGridViewButtonColumn3.Name = "dataGridViewButtonColumn3";
            this.dataGridViewButtonColumn3.ReadOnly = true;
            this.dataGridViewButtonColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewButtonColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewButtonColumn3.Width = 85;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "启动";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn4.Text = "启动";
            this.dataGridViewTextBoxColumn4.UseColumnTextForButtonValue = true;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "停止";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Text = "停止";
            this.Column1.UseColumnTextForButtonValue = true;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "打开";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Text = "打开";
            this.Column2.UseColumnTextForButtonValue = true;
            this.Column2.Width = 60;
            // 
            // colTaskKey
            // 
            this.colTaskKey.DataPropertyName = "Key";
            this.colTaskKey.HeaderText = "Key";
            this.colTaskKey.Name = "colTaskKey";
            this.colTaskKey.ReadOnly = true;
            this.colTaskKey.Visible = false;
            this.colTaskKey.Width = 5;
            // 
            // colTaskUrl
            // 
            this.colTaskUrl.DataPropertyName = "TaskUrl";
            this.colTaskUrl.HeaderText = "colTaskUrl";
            this.colTaskUrl.Name = "colTaskUrl";
            this.colTaskUrl.ReadOnly = true;
            this.colTaskUrl.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnStopTiming);
            this.groupBox4.Controls.Add(this.btnStartTiming);
            this.groupBox4.Controls.Add(this.chkAutoRefresh);
            this.groupBox4.Controls.Add(this.btnRefreshTask);
            this.groupBox4.Controls.Add(this.btnStopAllTask);
            this.groupBox4.Controls.Add(this.btnStartAllTask);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(930, 44);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "操作";
            // 
            // btnStopTiming
            // 
            this.btnStopTiming.Location = new System.Drawing.Point(127, 15);
            this.btnStopTiming.Name = "btnStopTiming";
            this.btnStopTiming.Size = new System.Drawing.Size(115, 23);
            this.btnStopTiming.TabIndex = 7;
            this.btnStopTiming.Text = "关闭定时监控";
            this.btnStopTiming.UseVisualStyleBackColor = true;
            this.btnStopTiming.Click += new System.EventHandler(this.btnStopTiming_Click);
            // 
            // btnStartTiming
            // 
            this.btnStartTiming.Location = new System.Drawing.Point(6, 15);
            this.btnStartTiming.Name = "btnStartTiming";
            this.btnStartTiming.Size = new System.Drawing.Size(115, 23);
            this.btnStartTiming.TabIndex = 6;
            this.btnStartTiming.Text = "启动定时监控";
            this.btnStartTiming.UseVisualStyleBackColor = true;
            this.btnStartTiming.Click += new System.EventHandler(this.btnStartTiming_Click);
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.AutoSize = true;
            this.chkAutoRefresh.Checked = true;
            this.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoRefresh.Location = new System.Drawing.Point(611, 19);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Size = new System.Drawing.Size(102, 16);
            this.chkAutoRefresh.TabIndex = 5;
            this.chkAutoRefresh.Text = "自动刷新(1秒)";
            this.chkAutoRefresh.UseVisualStyleBackColor = true;
            this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.chkAutoRefresh_CheckedChanged);
            // 
            // btnRefreshTask
            // 
            this.btnRefreshTask.Location = new System.Drawing.Point(490, 14);
            this.btnRefreshTask.Name = "btnRefreshTask";
            this.btnRefreshTask.Size = new System.Drawing.Size(115, 23);
            this.btnRefreshTask.TabIndex = 4;
            this.btnRefreshTask.Text = "刷新所有任务";
            this.btnRefreshTask.UseVisualStyleBackColor = true;
            this.btnRefreshTask.Click += new System.EventHandler(this.btnRefreshTask_Click);
            // 
            // btnStopAllTask
            // 
            this.btnStopAllTask.Location = new System.Drawing.Point(369, 14);
            this.btnStopAllTask.Name = "btnStopAllTask";
            this.btnStopAllTask.Size = new System.Drawing.Size(115, 23);
            this.btnStopAllTask.TabIndex = 3;
            this.btnStopAllTask.Text = "停止所有任务";
            this.btnStopAllTask.UseVisualStyleBackColor = true;
            this.btnStopAllTask.Click += new System.EventHandler(this.btnStopAllTask_Click);
            // 
            // btnStartAllTask
            // 
            this.btnStartAllTask.Location = new System.Drawing.Point(248, 15);
            this.btnStartAllTask.Name = "btnStartAllTask";
            this.btnStartAllTask.Size = new System.Drawing.Size(115, 23);
            this.btnStartAllTask.TabIndex = 2;
            this.btnStartAllTask.Text = "启动所有任务 ";
            this.btnStartAllTask.UseVisualStyleBackColor = true;
            this.btnStartAllTask.Click += new System.EventHandler(this.btnStartAllTask_Click);
            // 
            // timerTick
            // 
            this.timerTick.Interval = 5000;
            this.timerTick.Tick += new System.EventHandler(this.timerTick_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timerTask
            // 
            this.timerTask.Interval = 1000;
            this.timerTask.Tick += new System.EventHandler(this.timerTask_Tick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(286, 15);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Host
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 490);
            this.Controls.Add(this.tabPages);
            this.Name = "Host";
            this.Text = "微服务平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Host_FormClosing);
            this.Load += new System.EventHandler(this.Host_Load);
            this.tabPages.ResumeLayout(false);
            this.tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvlist)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabAdd.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPub;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabAdd;
        private System.Windows.Forms.TabPage tabList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvlist;
        private System.Windows.Forms.Timer timerTick;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn 状态;
        private System.Windows.Forms.DataGridViewButtonColumn 启动;
        private System.Windows.Forms.DataGridViewButtonColumn 重启;
        private System.Windows.Forms.DataGridViewButtonColumn 停止;
        private System.Windows.Forms.DataGridViewTextBoxColumn 错误信息;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txbFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txbEntry;
        private System.Windows.Forms.TextBox txbApiDirectory;
        private System.Windows.Forms.TextBox txbCommonDirectory;
        private System.Windows.Forms.TextBox txbUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkAutoRefresh;
        private System.Windows.Forms.Button btnRefreshTask;
        private System.Windows.Forms.Button btnStopAllTask;
        private System.Windows.Forms.Button btnStartAllTask;
        private System.Windows.Forms.Timer timerTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewButtonColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewButtonColumn3;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskUrl;
        private System.Windows.Forms.TextBox txtTimingDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPupTiming;
        private System.Windows.Forms.Button btnStopTiming;
        private System.Windows.Forms.Button btnStartTiming;
        private System.Windows.Forms.Button btnTest;
    }
}
