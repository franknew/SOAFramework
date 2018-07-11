namespace LoadTesting
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnZip = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.txbFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txbUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rabPost = new System.Windows.Forms.RadioButton();
            this.rabGet = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFromFile = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rabUpload = new System.Windows.Forms.RadioButton();
            this.rdoUrlencoded = new System.Windows.Forms.RadioButton();
            this.rabXml = new System.Windows.Forms.RadioButton();
            this.rabJson = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbPost = new System.Windows.Forms.TextBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.Url = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxRequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.btnZip);
            this.groupBox1.Controls.Add(this.btnChoose);
            this.groupBox1.Controls.Add(this.txbFile);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnRun);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1491, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(553, 21);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "加载";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnZip
            // 
            this.btnZip.Location = new System.Drawing.Point(634, 21);
            this.btnZip.Name = "btnZip";
            this.btnZip.Size = new System.Drawing.Size(75, 23);
            this.btnZip.TabIndex = 4;
            this.btnZip.Text = "压缩";
            this.btnZip.UseVisualStyleBackColor = true;
            this.btnZip.Click += new System.EventHandler(this.btnZip_Click);
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(471, 20);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 3;
            this.btnChoose.Text = "选择";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.txbChoose_Click);
            // 
            // txbFile
            // 
            this.txbFile.Location = new System.Drawing.Point(175, 21);
            this.txbFile.Name = "txbFile";
            this.txbFile.Size = new System.Drawing.Size(290, 21);
            this.txbFile.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "飞行员视频";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 20);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "运行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txbUrl
            // 
            this.txbUrl.Location = new System.Drawing.Point(92, 20);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(851, 21);
            this.txbUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "method";
            // 
            // rabPost
            // 
            this.rabPost.AutoSize = true;
            this.rabPost.Checked = true;
            this.rabPost.Location = new System.Drawing.Point(40, 16);
            this.rabPost.Name = "rabPost";
            this.rabPost.Size = new System.Drawing.Size(47, 16);
            this.rabPost.TabIndex = 4;
            this.rabPost.TabStop = true;
            this.rabPost.Text = "post";
            this.rabPost.UseVisualStyleBackColor = true;
            // 
            // rabGet
            // 
            this.rabGet.AutoSize = true;
            this.rabGet.Location = new System.Drawing.Point(121, 16);
            this.rabGet.Name = "rabGet";
            this.rabGet.Size = new System.Drawing.Size(41, 16);
            this.rabGet.TabIndex = 5;
            this.rabGet.Text = "get";
            this.rabGet.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "请求数";
            // 
            // numCount
            // 
            this.numCount.Location = new System.Drawing.Point(92, 99);
            this.numCount.Maximum = new decimal(new int[] {
            1661992959,
            1808227885,
            5,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(123, 21);
            this.numCount.TabIndex = 7;
            this.numCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFromFile);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txbPost);
            this.groupBox2.Controls.Add(this.txbUrl);
            this.groupBox2.Controls.Add(this.numCount);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1491, 253);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // btnFromFile
            // 
            this.btnFromFile.Location = new System.Drawing.Point(221, 99);
            this.btnFromFile.Name = "btnFromFile";
            this.btnFromFile.Size = new System.Drawing.Size(75, 23);
            this.btnFromFile.TabIndex = 6;
            this.btnFromFile.Text = "从文件";
            this.btnFromFile.UseVisualStyleBackColor = true;
            this.btnFromFile.Click += new System.EventHandler(this.btnFromFile_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rabUpload);
            this.groupBox4.Controls.Add(this.rdoUrlencoded);
            this.groupBox4.Controls.Add(this.rabXml);
            this.groupBox4.Controls.Add(this.rabJson);
            this.groupBox4.Location = new System.Drawing.Point(415, 51);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1045, 38);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            // 
            // rabUpload
            // 
            this.rabUpload.AutoSize = true;
            this.rabUpload.Location = new System.Drawing.Point(484, 13);
            this.rabUpload.Name = "rabUpload";
            this.rabUpload.Size = new System.Drawing.Size(137, 16);
            this.rabUpload.TabIndex = 13;
            this.rabUpload.TabStop = true;
            this.rabUpload.Text = "multipart/form-data";
            this.rabUpload.UseVisualStyleBackColor = true;
            // 
            // rdoUrlencoded
            // 
            this.rdoUrlencoded.AutoSize = true;
            this.rdoUrlencoded.Location = new System.Drawing.Point(246, 14);
            this.rdoUrlencoded.Name = "rdoUrlencoded";
            this.rdoUrlencoded.Size = new System.Drawing.Size(221, 16);
            this.rdoUrlencoded.TabIndex = 12;
            this.rdoUrlencoded.Text = "application/x-www-form-urlencoded";
            this.rdoUrlencoded.UseVisualStyleBackColor = true;
            // 
            // rabXml
            // 
            this.rabXml.AutoSize = true;
            this.rabXml.Location = new System.Drawing.Point(157, 16);
            this.rabXml.Name = "rabXml";
            this.rabXml.Size = new System.Drawing.Size(71, 16);
            this.rabXml.TabIndex = 11;
            this.rabXml.Text = "text/xml";
            this.rabXml.UseVisualStyleBackColor = true;
            // 
            // rabJson
            // 
            this.rabJson.AutoSize = true;
            this.rabJson.Checked = true;
            this.rabJson.Location = new System.Drawing.Point(23, 16);
            this.rabJson.Name = "rabJson";
            this.rabJson.Size = new System.Drawing.Size(119, 16);
            this.rabJson.TabIndex = 10;
            this.rabJson.TabStop = true;
            this.rabJson.Text = "application/json";
            this.rabJson.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rabPost);
            this.groupBox3.Controls.Add(this.rabGet);
            this.groupBox3.Location = new System.Drawing.Point(92, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 38);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(317, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "content type";
            // 
            // txbPost
            // 
            this.txbPost.Location = new System.Drawing.Point(47, 136);
            this.txbPost.Multiline = true;
            this.txbPost.Name = "txbPost";
            this.txbPost.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbPost.Size = new System.Drawing.Size(896, 111);
            this.txbPost.TabIndex = 8;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Url,
            this.Method,
            this.Count,
            this.Time,
            this.MaxRequestTime,
            this.AverageCount,
            this.Result});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 303);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(1491, 429);
            this.dgvData.TabIndex = 9;
            // 
            // Url
            // 
            this.Url.DataPropertyName = "Url";
            this.Url.HeaderText = "Url";
            this.Url.Name = "Url";
            this.Url.ReadOnly = true;
            this.Url.Width = 500;
            // 
            // Method
            // 
            this.Method.DataPropertyName = "Method";
            this.Method.HeaderText = "Method";
            this.Method.Name = "Method";
            this.Method.ReadOnly = true;
            // 
            // Count
            // 
            this.Count.DataPropertyName = "Count";
            this.Count.HeaderText = "Count";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.DataPropertyName = "Time";
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // MaxRequestTime
            // 
            this.MaxRequestTime.DataPropertyName = "MaxRequestTime";
            this.MaxRequestTime.HeaderText = "MaxRequestTime";
            this.MaxRequestTime.Name = "MaxRequestTime";
            this.MaxRequestTime.ReadOnly = true;
            // 
            // AverageCount
            // 
            this.AverageCount.DataPropertyName = "AverageCount";
            this.AverageCount.HeaderText = "AverageCount";
            this.AverageCount.Name = "AverageCount";
            this.AverageCount.ReadOnly = true;
            // 
            // Result
            // 
            this.Result.DataPropertyName = "Result";
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            // 
            // progressbar
            // 
            this.progressbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressbar.Location = new System.Drawing.Point(199, 364);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(1048, 23);
            this.progressbar.TabIndex = 10;
            this.progressbar.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(199, 321);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1048, 23);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1491, 732);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "LoadTesting";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rabPost;
        private System.Windows.Forms.RadioButton rabGet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txbPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Url;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxRequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rabJson;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rabXml;
        private System.Windows.Forms.RadioButton rdoUrlencoded;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.TextBox txbFile;
        private System.Windows.Forms.Button btnZip;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnFromFile;
        private System.Windows.Forms.RadioButton rabUpload;
    }
}

