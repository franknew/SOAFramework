namespace WinformTest
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddToFirewall = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.txbName = new System.Windows.Forms.TextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txbData = new System.Windows.Forms.TextBox();
            this.btnInvoke = new System.Windows.Forms.Button();
            this.lblContent = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnmq = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txbJarPath = new System.Windows.Forms.TextBox();
            this.txbMessage = new System.Windows.Forms.TextBox();
            this.btnConsume = new System.Windows.Forms.Button();
            this.txbConsume = new System.Windows.Forms.TextBox();
            this.btnJVM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "memcached";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddToFirewall
            // 
            this.btnAddToFirewall.Location = new System.Drawing.Point(12, 467);
            this.btnAddToFirewall.Name = "btnAddToFirewall";
            this.btnAddToFirewall.Size = new System.Drawing.Size(100, 23);
            this.btnAddToFirewall.TabIndex = 1;
            this.btnAddToFirewall.Text = "addToFirewall";
            this.btnAddToFirewall.UseVisualStyleBackColor = true;
            this.btnAddToFirewall.Click += new System.EventHandler(this.btnAddToFirewall_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(118, 427);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(100, 21);
            this.txbPort.TabIndex = 2;
            // 
            // txbName
            // 
            this.txbName.Location = new System.Drawing.Point(12, 427);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(100, 21);
            this.txbName.TabIndex = 3;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(118, 467);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txbData
            // 
            this.txbData.Location = new System.Drawing.Point(330, 14);
            this.txbData.Multiline = true;
            this.txbData.Name = "txbData";
            this.txbData.Size = new System.Drawing.Size(470, 214);
            this.txbData.TabIndex = 5;
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(330, 234);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(75, 23);
            this.btnInvoke.TabIndex = 6;
            this.btnInvoke.Text = "invoke";
            this.btnInvoke.UseVisualStyleBackColor = true;
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // lblContent
            // 
            this.lblContent.Location = new System.Drawing.Point(330, 264);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(470, 478);
            this.lblContent.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 523);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(206, 21);
            this.textBox1.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 550);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "all service";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnmq
            // 
            this.btnmq.Location = new System.Drawing.Point(830, 719);
            this.btnmq.Name = "btnmq";
            this.btnmq.Size = new System.Drawing.Size(141, 23);
            this.btnmq.TabIndex = 10;
            this.btnmq.Text = "produce message";
            this.btnmq.UseVisualStyleBackColor = true;
            this.btnmq.Click += new System.EventHandler(this.btnmq_Click);
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(224, 521);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(100, 23);
            this.btnChoose.TabIndex = 11;
            this.btnChoose.Text = "choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(224, 550);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 23);
            this.btnCopy.TabIndex = 12;
            this.btnCopy.Text = "copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(12, 138);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 13;
            this.btnConvert.Text = "jar to dll";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txbJarPath
            // 
            this.txbJarPath.Location = new System.Drawing.Point(13, 111);
            this.txbJarPath.Name = "txbJarPath";
            this.txbJarPath.Size = new System.Drawing.Size(311, 21);
            this.txbJarPath.TabIndex = 14;
            this.txbJarPath.Text = "E:\\jartodll\\jar";
            // 
            // txbMessage
            // 
            this.txbMessage.Location = new System.Drawing.Point(830, 692);
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Size = new System.Drawing.Size(522, 21);
            this.txbMessage.TabIndex = 15;
            // 
            // btnConsume
            // 
            this.btnConsume.Location = new System.Drawing.Point(830, 559);
            this.btnConsume.Name = "btnConsume";
            this.btnConsume.Size = new System.Drawing.Size(141, 23);
            this.btnConsume.TabIndex = 16;
            this.btnConsume.Text = "consume message";
            this.btnConsume.UseVisualStyleBackColor = true;
            this.btnConsume.Click += new System.EventHandler(this.btnConsume_Click);
            // 
            // txbConsume
            // 
            this.txbConsume.Location = new System.Drawing.Point(830, 342);
            this.txbConsume.Multiline = true;
            this.txbConsume.Name = "txbConsume";
            this.txbConsume.Size = new System.Drawing.Size(638, 211);
            this.txbConsume.TabIndex = 17;
            // 
            // btnJVM
            // 
            this.btnJVM.Location = new System.Drawing.Point(13, 167);
            this.btnJVM.Name = "btnJVM";
            this.btnJVM.Size = new System.Drawing.Size(75, 23);
            this.btnJVM.TabIndex = 18;
            this.btnJVM.Text = "jvm";
            this.btnJVM.UseVisualStyleBackColor = true;
            this.btnJVM.Click += new System.EventHandler(this.btnJVM_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1617, 785);
            this.Controls.Add(this.btnJVM);
            this.Controls.Add(this.txbConsume);
            this.Controls.Add(this.btnConsume);
            this.Controls.Add(this.txbMessage);
            this.Controls.Add(this.txbJarPath);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.btnmq);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.btnInvoke);
            this.Controls.Add(this.txbData);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.btnAddToFirewall);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAddToFirewall;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TextBox txbData;
        private System.Windows.Forms.Button btnInvoke;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnmq;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txbJarPath;
        private System.Windows.Forms.TextBox txbMessage;
        private System.Windows.Forms.Button btnConsume;
        private System.Windows.Forms.TextBox txbConsume;
        private System.Windows.Forms.Button btnJVM;
    }
}

