using SOAFramework.Service.UI;
namespace SOAFramework.Server.UI
{
    partial class ServerUI : BaseUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // gpbCondition
            // 
            this.gpbCondition.Size = new System.Drawing.Size(633, 10);
            // 
            // txbMessage
            // 
            this.txbMessage.Location = new System.Drawing.Point(0, 51);
            this.txbMessage.Size = new System.Drawing.Size(633, 312);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            // 
            // ServerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 385);
            this.Name = "ServerUI";
            this.Text = "SOA服务端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}