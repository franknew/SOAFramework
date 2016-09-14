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
            this.btnPub = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.tabList = new System.Windows.Forms.TabPage();
            this.tabPages.SuspendLayout();
            this.tabAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPub
            // 
            this.btnPub.Location = new System.Drawing.Point(23, 6);
            this.btnPub.Name = "btnPub";
            this.btnPub.Size = new System.Drawing.Size(75, 23);
            this.btnPub.TabIndex = 0;
            this.btnPub.Text = "部署并启动";
            this.btnPub.UseVisualStyleBackColor = true;
            this.btnPub.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(23, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(429, 21);
            this.textBox1.TabIndex = 1;
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.tabList);
            this.tabPages.Controls.Add(this.tabAdd);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(944, 490);
            this.tabPages.TabIndex = 2;
            // 
            // tabAdd
            // 
            this.tabAdd.Controls.Add(this.btnPub);
            this.tabAdd.Controls.Add(this.textBox1);
            this.tabAdd.Location = new System.Drawing.Point(4, 22);
            this.tabAdd.Name = "tabAdd";
            this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdd.Size = new System.Drawing.Size(936, 464);
            this.tabAdd.TabIndex = 0;
            this.tabAdd.Text = "接口部署";
            this.tabAdd.UseVisualStyleBackColor = true;
            // 
            // tabList
            // 
            this.tabList.Location = new System.Drawing.Point(4, 22);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(3);
            this.tabList.Size = new System.Drawing.Size(936, 464);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "接口状态监控";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // Host
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 490);
            this.Controls.Add(this.tabPages);
            this.Name = "Host";
            this.Text = "微服务平台";
            this.tabPages.ResumeLayout(false);
            this.tabAdd.ResumeLayout(false);
            this.tabAdd.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPub;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabAdd;
        private System.Windows.Forms.TabPage tabList;
    }
}

