namespace DB
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
            this.btnSQL = new System.Windows.Forms.Button();
            this.txbSQL = new System.Windows.Forms.TextBox();
            this.btnData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSQL
            // 
            this.btnSQL.Location = new System.Drawing.Point(332, 500);
            this.btnSQL.Name = "btnSQL";
            this.btnSQL.Size = new System.Drawing.Size(75, 23);
            this.btnSQL.TabIndex = 0;
            this.btnSQL.Text = "exec sql";
            this.btnSQL.UseVisualStyleBackColor = true;
            this.btnSQL.Click += new System.EventHandler(this.btnSQL_Click);
            // 
            // txbSQL
            // 
            this.txbSQL.Location = new System.Drawing.Point(12, 12);
            this.txbSQL.Multiline = true;
            this.txbSQL.Name = "txbSQL";
            this.txbSQL.Size = new System.Drawing.Size(758, 468);
            this.txbSQL.TabIndex = 1;
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(199, 500);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(75, 23);
            this.btnData.TabIndex = 2;
            this.btnData.Text = "get data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 598);
            this.Controls.Add(this.btnData);
            this.Controls.Add(this.txbSQL);
            this.Controls.Add(this.btnSQL);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSQL;
        private System.Windows.Forms.TextBox txbSQL;
        private System.Windows.Forms.Button btnData;
    }
}

