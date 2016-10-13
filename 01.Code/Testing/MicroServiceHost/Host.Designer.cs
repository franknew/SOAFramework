namespace MicroServiceTesting
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.timerTick = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.启动 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.重启 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.停止 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.错误信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txbUrl = new System.Windows.Forms.TextBox();
            this.txbCommonDirectory = new System.Windows.Forms.TextBox();
            this.txbApiDirectory = new System.Windows.Forms.TextBox();
            this.txbEntry = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.tabPages.SuspendLayout();
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvlist)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabAdd.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(944, 490);
            this.tabPages.TabIndex = 2;
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
            // groupBox1
            // 
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
            // tabLog
            // 
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Size = new System.Drawing.Size(936, 464);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "日志监控";
            this.tabLog.UseVisualStyleBackColor = true;
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
            // txbFile
            // 
            this.txbFile.Location = new System.Drawing.Point(135, 97);
            this.txbFile.Name = "txbFile";
            this.txbFile.ReadOnly = true;
            this.txbFile.Size = new System.Drawing.Size(674, 21);
            this.txbFile.TabIndex = 4;
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
            // groupBox2
            // 
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
            // groupBox3
            // 
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "公共目录";
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "服务入口模板文件";
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
            // txbUrl
            // 
            this.txbUrl.Location = new System.Drawing.Point(135, 32);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(728, 21);
            this.txbUrl.TabIndex = 7;
            // 
            // txbCommonDirectory
            // 
            this.txbCommonDirectory.Location = new System.Drawing.Point(135, 66);
            this.txbCommonDirectory.Name = "txbCommonDirectory";
            this.txbCommonDirectory.Size = new System.Drawing.Size(728, 21);
            this.txbCommonDirectory.TabIndex = 8;
            // 
            // txbApiDirectory
            // 
            this.txbApiDirectory.Location = new System.Drawing.Point(135, 108);
            this.txbApiDirectory.Name = "txbApiDirectory";
            this.txbApiDirectory.Size = new System.Drawing.Size(728, 21);
            this.txbApiDirectory.TabIndex = 9;
            // 
            // txbEntry
            // 
            this.txbEntry.Location = new System.Drawing.Point(135, 152);
            this.txbEntry.Name = "txbEntry";
            this.txbEntry.Size = new System.Drawing.Size(728, 21);
            this.txbEntry.TabIndex = 10;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(91, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "部署并启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
    }
}

