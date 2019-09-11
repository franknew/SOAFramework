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
            this.btnRedisSet = new System.Windows.Forms.Button();
            this.btnDotNetty = new System.Windows.Forms.Button();
            this.btnDalQuery = new System.Windows.Forms.Button();
            this.btnFTS = new System.Windows.Forms.Button();
            this.btnSTF = new System.Windows.Forms.Button();
            this.btnSDKTesting = new System.Windows.Forms.Button();
            this.btnChangeType = new System.Windows.Forms.Button();
            this.btnsql = new System.Windows.Forms.Button();
            this.btnDynamic = new System.Windows.Forms.Button();
            this.btnZipPackage = new System.Windows.Forms.Button();
            this.btnJWTEncode = new System.Windows.Forms.Button();
            this.txbHeader = new System.Windows.Forms.TextBox();
            this.txbPayload = new System.Windows.Forms.TextBox();
            this.txbSecret = new System.Windows.Forms.TextBox();
            this.btnJWTDecode = new System.Windows.Forms.Button();
            this.btnLogAsync = new System.Windows.Forms.Button();
            this.btnLogSync = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCpu = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalMem = new System.Windows.Forms.Label();
            this.lblUsedMem = new System.Windows.Forms.Label();
            this.btnSms = new System.Windows.Forms.Button();
            this.btnProxy = new System.Windows.Forms.Button();
            this.btnDalTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 16);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "memcached";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddToFirewall
            // 
            this.btnAddToFirewall.Location = new System.Drawing.Point(16, 584);
            this.btnAddToFirewall.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddToFirewall.Name = "btnAddToFirewall";
            this.btnAddToFirewall.Size = new System.Drawing.Size(133, 29);
            this.btnAddToFirewall.TabIndex = 1;
            this.btnAddToFirewall.Text = "addToFirewall";
            this.btnAddToFirewall.UseVisualStyleBackColor = true;
            this.btnAddToFirewall.Click += new System.EventHandler(this.btnAddToFirewall_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(157, 534);
            this.txbPort.Margin = new System.Windows.Forms.Padding(4);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(132, 25);
            this.txbPort.TabIndex = 2;
            // 
            // txbName
            // 
            this.txbName.Location = new System.Drawing.Point(16, 534);
            this.txbName.Margin = new System.Windows.Forms.Padding(4);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(132, 25);
            this.txbName.TabIndex = 3;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(157, 584);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(133, 29);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txbData
            // 
            this.txbData.Location = new System.Drawing.Point(440, 18);
            this.txbData.Margin = new System.Windows.Forms.Padding(4);
            this.txbData.Multiline = true;
            this.txbData.Name = "txbData";
            this.txbData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbData.Size = new System.Drawing.Size(625, 909);
            this.txbData.TabIndex = 5;
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(440, 938);
            this.btnInvoke.Margin = new System.Windows.Forms.Padding(4);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(100, 29);
            this.btnInvoke.TabIndex = 6;
            this.btnInvoke.Text = "invoke";
            this.btnInvoke.UseVisualStyleBackColor = true;
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 654);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(273, 25);
            this.textBox1.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 688);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 29);
            this.button2.TabIndex = 9;
            this.button2.Text = "all service";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnmq
            // 
            this.btnmq.Location = new System.Drawing.Point(1107, 899);
            this.btnmq.Margin = new System.Windows.Forms.Padding(4);
            this.btnmq.Name = "btnmq";
            this.btnmq.Size = new System.Drawing.Size(188, 29);
            this.btnmq.TabIndex = 10;
            this.btnmq.Text = "produce message";
            this.btnmq.UseVisualStyleBackColor = true;
            this.btnmq.Click += new System.EventHandler(this.btnmq_Click);
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(299, 651);
            this.btnChoose.Margin = new System.Windows.Forms.Padding(4);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(133, 29);
            this.btnChoose.TabIndex = 11;
            this.btnChoose.Text = "choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(299, 688);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(133, 29);
            this.btnCopy.TabIndex = 12;
            this.btnCopy.Text = "copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(16, 172);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(100, 29);
            this.btnConvert.TabIndex = 13;
            this.btnConvert.Text = "jar to dll";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txbJarPath
            // 
            this.txbJarPath.Location = new System.Drawing.Point(17, 139);
            this.txbJarPath.Margin = new System.Windows.Forms.Padding(4);
            this.txbJarPath.Name = "txbJarPath";
            this.txbJarPath.Size = new System.Drawing.Size(413, 25);
            this.txbJarPath.TabIndex = 14;
            this.txbJarPath.Text = "E:\\jartodll\\jar";
            // 
            // txbMessage
            // 
            this.txbMessage.Location = new System.Drawing.Point(1107, 865);
            this.txbMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Size = new System.Drawing.Size(695, 25);
            this.txbMessage.TabIndex = 15;
            // 
            // btnConsume
            // 
            this.btnConsume.Location = new System.Drawing.Point(1107, 699);
            this.btnConsume.Margin = new System.Windows.Forms.Padding(4);
            this.btnConsume.Name = "btnConsume";
            this.btnConsume.Size = new System.Drawing.Size(188, 29);
            this.btnConsume.TabIndex = 16;
            this.btnConsume.Text = "consume message";
            this.btnConsume.UseVisualStyleBackColor = true;
            this.btnConsume.Click += new System.EventHandler(this.btnConsume_Click);
            // 
            // txbConsume
            // 
            this.txbConsume.Location = new System.Drawing.Point(1107, 428);
            this.txbConsume.Margin = new System.Windows.Forms.Padding(4);
            this.txbConsume.Multiline = true;
            this.txbConsume.Name = "txbConsume";
            this.txbConsume.Size = new System.Drawing.Size(849, 263);
            this.txbConsume.TabIndex = 17;
            // 
            // btnJVM
            // 
            this.btnJVM.Location = new System.Drawing.Point(17, 209);
            this.btnJVM.Margin = new System.Windows.Forms.Padding(4);
            this.btnJVM.Name = "btnJVM";
            this.btnJVM.Size = new System.Drawing.Size(100, 29);
            this.btnJVM.TabIndex = 18;
            this.btnJVM.Text = "jvm";
            this.btnJVM.UseVisualStyleBackColor = true;
            this.btnJVM.Click += new System.EventHandler(this.btnJVM_Click);
            // 
            // btnRedisSet
            // 
            this.btnRedisSet.Location = new System.Drawing.Point(1107, 18);
            this.btnRedisSet.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedisSet.Name = "btnRedisSet";
            this.btnRedisSet.Size = new System.Drawing.Size(100, 29);
            this.btnRedisSet.TabIndex = 19;
            this.btnRedisSet.Text = "redis set";
            this.btnRedisSet.UseVisualStyleBackColor = true;
            this.btnRedisSet.Click += new System.EventHandler(this.btnRedisSet_Click);
            // 
            // btnDotNetty
            // 
            this.btnDotNetty.Location = new System.Drawing.Point(1107, 391);
            this.btnDotNetty.Margin = new System.Windows.Forms.Padding(4);
            this.btnDotNetty.Name = "btnDotNetty";
            this.btnDotNetty.Size = new System.Drawing.Size(140, 29);
            this.btnDotNetty.TabIndex = 20;
            this.btnDotNetty.Text = "dotnetty http";
            this.btnDotNetty.UseVisualStyleBackColor = true;
            this.btnDotNetty.Click += new System.EventHandler(this.btnDotNetty_Click);
            // 
            // btnDalQuery
            // 
            this.btnDalQuery.Location = new System.Drawing.Point(1107, 54);
            this.btnDalQuery.Margin = new System.Windows.Forms.Padding(4);
            this.btnDalQuery.Name = "btnDalQuery";
            this.btnDalQuery.Size = new System.Drawing.Size(100, 29);
            this.btnDalQuery.TabIndex = 21;
            this.btnDalQuery.Text = "dal query";
            this.btnDalQuery.UseVisualStyleBackColor = true;
            this.btnDalQuery.Click += new System.EventHandler(this.btnDalQuery_Click);
            // 
            // btnFTS
            // 
            this.btnFTS.Location = new System.Drawing.Point(548, 938);
            this.btnFTS.Margin = new System.Windows.Forms.Padding(4);
            this.btnFTS.Name = "btnFTS";
            this.btnFTS.Size = new System.Drawing.Size(160, 29);
            this.btnFTS.TabIndex = 22;
            this.btnFTS.Text = "File To String";
            this.btnFTS.UseVisualStyleBackColor = true;
            this.btnFTS.Click += new System.EventHandler(this.btnFTS_Click);
            // 
            // btnSTF
            // 
            this.btnSTF.Location = new System.Drawing.Point(716, 935);
            this.btnSTF.Margin = new System.Windows.Forms.Padding(4);
            this.btnSTF.Name = "btnSTF";
            this.btnSTF.Size = new System.Drawing.Size(160, 29);
            this.btnSTF.TabIndex = 23;
            this.btnSTF.Text = "string to file";
            this.btnSTF.UseVisualStyleBackColor = true;
            this.btnSTF.Click += new System.EventHandler(this.btnSTF_Click);
            // 
            // btnSDKTesting
            // 
            this.btnSDKTesting.Location = new System.Drawing.Point(1107, 90);
            this.btnSDKTesting.Margin = new System.Windows.Forms.Padding(4);
            this.btnSDKTesting.Name = "btnSDKTesting";
            this.btnSDKTesting.Size = new System.Drawing.Size(115, 31);
            this.btnSDKTesting.TabIndex = 24;
            this.btnSDKTesting.Text = "sdk testing";
            this.btnSDKTesting.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSDKTesting.UseVisualStyleBackColor = true;
            this.btnSDKTesting.Click += new System.EventHandler(this.btnSDKTesting_Click);
            // 
            // btnChangeType
            // 
            this.btnChangeType.Location = new System.Drawing.Point(1107, 352);
            this.btnChangeType.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangeType.Name = "btnChangeType";
            this.btnChangeType.Size = new System.Drawing.Size(152, 31);
            this.btnChangeType.TabIndex = 25;
            this.btnChangeType.Text = "change type test";
            this.btnChangeType.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnChangeType.UseVisualStyleBackColor = true;
            this.btnChangeType.Click += new System.EventHandler(this.btnChangeType_Click);
            // 
            // btnsql
            // 
            this.btnsql.Location = new System.Drawing.Point(1107, 314);
            this.btnsql.Margin = new System.Windows.Forms.Padding(4);
            this.btnsql.Name = "btnsql";
            this.btnsql.Size = new System.Drawing.Size(100, 31);
            this.btnsql.TabIndex = 26;
            this.btnsql.Text = "exec sql";
            this.btnsql.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnsql.UseVisualStyleBackColor = true;
            this.btnsql.Click += new System.EventHandler(this.btnsql_Click);
            // 
            // btnDynamic
            // 
            this.btnDynamic.Location = new System.Drawing.Point(1107, 275);
            this.btnDynamic.Margin = new System.Windows.Forms.Padding(4);
            this.btnDynamic.Name = "btnDynamic";
            this.btnDynamic.Size = new System.Drawing.Size(100, 31);
            this.btnDynamic.TabIndex = 27;
            this.btnDynamic.Text = "dynamic";
            this.btnDynamic.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnDynamic.UseVisualStyleBackColor = true;
            this.btnDynamic.Click += new System.EventHandler(this.btnDynamic_Click);
            // 
            // btnZipPackage
            // 
            this.btnZipPackage.Location = new System.Drawing.Point(16, 245);
            this.btnZipPackage.Margin = new System.Windows.Forms.Padding(4);
            this.btnZipPackage.Name = "btnZipPackage";
            this.btnZipPackage.Size = new System.Drawing.Size(133, 29);
            this.btnZipPackage.TabIndex = 28;
            this.btnZipPackage.Text = "zip package";
            this.btnZipPackage.UseVisualStyleBackColor = true;
            this.btnZipPackage.Click += new System.EventHandler(this.btnZipPackage_Click);
            // 
            // btnJWTEncode
            // 
            this.btnJWTEncode.Location = new System.Drawing.Point(1284, 16);
            this.btnJWTEncode.Margin = new System.Windows.Forms.Padding(4);
            this.btnJWTEncode.Name = "btnJWTEncode";
            this.btnJWTEncode.Size = new System.Drawing.Size(115, 31);
            this.btnJWTEncode.TabIndex = 29;
            this.btnJWTEncode.Text = "jwt encode";
            this.btnJWTEncode.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnJWTEncode.UseVisualStyleBackColor = true;
            this.btnJWTEncode.Click += new System.EventHandler(this.btnJWT_Click);
            // 
            // txbHeader
            // 
            this.txbHeader.Location = new System.Drawing.Point(1447, 18);
            this.txbHeader.Margin = new System.Windows.Forms.Padding(4);
            this.txbHeader.Multiline = true;
            this.txbHeader.Name = "txbHeader";
            this.txbHeader.Size = new System.Drawing.Size(521, 85);
            this.txbHeader.TabIndex = 30;
            // 
            // txbPayload
            // 
            this.txbPayload.Location = new System.Drawing.Point(1447, 115);
            this.txbPayload.Margin = new System.Windows.Forms.Padding(4);
            this.txbPayload.Multiline = true;
            this.txbPayload.Name = "txbPayload";
            this.txbPayload.Size = new System.Drawing.Size(521, 85);
            this.txbPayload.TabIndex = 31;
            this.txbPayload.Text = "{\'id\':\'1\',\'name\':\'frank\'}";
            // 
            // txbSecret
            // 
            this.txbSecret.Location = new System.Drawing.Point(1447, 211);
            this.txbSecret.Margin = new System.Windows.Forms.Padding(4);
            this.txbSecret.Multiline = true;
            this.txbSecret.Name = "txbSecret";
            this.txbSecret.Size = new System.Drawing.Size(521, 85);
            this.txbSecret.TabIndex = 32;
            this.txbSecret.Text = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            // 
            // btnJWTDecode
            // 
            this.btnJWTDecode.Location = new System.Drawing.Point(1284, 72);
            this.btnJWTDecode.Margin = new System.Windows.Forms.Padding(4);
            this.btnJWTDecode.Name = "btnJWTDecode";
            this.btnJWTDecode.Size = new System.Drawing.Size(115, 31);
            this.btnJWTDecode.TabIndex = 33;
            this.btnJWTDecode.Text = "jwt decode";
            this.btnJWTDecode.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnJWTDecode.UseVisualStyleBackColor = true;
            this.btnJWTDecode.Click += new System.EventHandler(this.btnJWTDecode_Click);
            // 
            // btnLogAsync
            // 
            this.btnLogAsync.Location = new System.Drawing.Point(1107, 236);
            this.btnLogAsync.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogAsync.Name = "btnLogAsync";
            this.btnLogAsync.Size = new System.Drawing.Size(100, 31);
            this.btnLogAsync.TabIndex = 34;
            this.btnLogAsync.Text = "log async test";
            this.btnLogAsync.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnLogAsync.UseVisualStyleBackColor = true;
            this.btnLogAsync.Click += new System.EventHandler(this.btnLogAsync_Click);
            // 
            // btnLogSync
            // 
            this.btnLogSync.Location = new System.Drawing.Point(1107, 198);
            this.btnLogSync.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogSync.Name = "btnLogSync";
            this.btnLogSync.Size = new System.Drawing.Size(100, 31);
            this.btnLogSync.TabIndex = 35;
            this.btnLogSync.Text = "log sync";
            this.btnLogSync.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnLogSync.UseVisualStyleBackColor = true;
            this.btnLogSync.Click += new System.EventHandler(this.btnLogSync_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 738);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 36;
            this.label1.Text = "cpu：";
            // 
            // lblCpu
            // 
            this.lblCpu.AutoSize = true;
            this.lblCpu.Location = new System.Drawing.Point(117, 738);
            this.lblCpu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCpu.Name = "lblCpu";
            this.lblCpu.Size = new System.Drawing.Size(0, 15);
            this.lblCpu.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 776);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 38;
            this.label2.Text = "总内存：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 821);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 39;
            this.label3.Text = "已用内存：";
            // 
            // lblTotalMem
            // 
            this.lblTotalMem.AutoSize = true;
            this.lblTotalMem.Location = new System.Drawing.Point(117, 776);
            this.lblTotalMem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalMem.Name = "lblTotalMem";
            this.lblTotalMem.Size = new System.Drawing.Size(0, 15);
            this.lblTotalMem.TabIndex = 40;
            // 
            // lblUsedMem
            // 
            this.lblUsedMem.AutoSize = true;
            this.lblUsedMem.Location = new System.Drawing.Point(117, 821);
            this.lblUsedMem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsedMem.Name = "lblUsedMem";
            this.lblUsedMem.Size = new System.Drawing.Size(0, 15);
            this.lblUsedMem.TabIndex = 41;
            // 
            // btnSms
            // 
            this.btnSms.Location = new System.Drawing.Point(1107, 738);
            this.btnSms.Margin = new System.Windows.Forms.Padding(4);
            this.btnSms.Name = "btnSms";
            this.btnSms.Size = new System.Drawing.Size(188, 29);
            this.btnSms.TabIndex = 42;
            this.btnSms.Text = "send sms";
            this.btnSms.UseVisualStyleBackColor = true;
            this.btnSms.Click += new System.EventHandler(this.btnSms_Click);
            // 
            // btnProxy
            // 
            this.btnProxy.Location = new System.Drawing.Point(16, 504);
            this.btnProxy.Name = "btnProxy";
            this.btnProxy.Size = new System.Drawing.Size(133, 23);
            this.btnProxy.TabIndex = 43;
            this.btnProxy.Text = "proxy test";
            this.btnProxy.UseVisualStyleBackColor = true;
            this.btnProxy.Click += new System.EventHandler(this.btnProxy_Click);
            // 
            // btnDalTest
            // 
            this.btnDalTest.Location = new System.Drawing.Point(17, 468);
            this.btnDalTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnDalTest.Name = "btnDalTest";
            this.btnDalTest.Size = new System.Drawing.Size(100, 29);
            this.btnDalTest.TabIndex = 44;
            this.btnDalTest.Text = "dal test";
            this.btnDalTest.UseVisualStyleBackColor = true;
            this.btnDalTest.Click += new System.EventHandler(this.btnDalTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1914, 981);
            this.Controls.Add(this.btnDalTest);
            this.Controls.Add(this.btnProxy);
            this.Controls.Add(this.btnSms);
            this.Controls.Add(this.lblUsedMem);
            this.Controls.Add(this.lblTotalMem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCpu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogSync);
            this.Controls.Add(this.btnLogAsync);
            this.Controls.Add(this.btnJWTDecode);
            this.Controls.Add(this.txbSecret);
            this.Controls.Add(this.txbPayload);
            this.Controls.Add(this.txbHeader);
            this.Controls.Add(this.btnJWTEncode);
            this.Controls.Add(this.btnZipPackage);
            this.Controls.Add(this.btnDynamic);
            this.Controls.Add(this.btnsql);
            this.Controls.Add(this.btnChangeType);
            this.Controls.Add(this.btnSDKTesting);
            this.Controls.Add(this.btnSTF);
            this.Controls.Add(this.btnFTS);
            this.Controls.Add(this.btnDalQuery);
            this.Controls.Add(this.btnDotNetty);
            this.Controls.Add(this.btnRedisSet);
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
            this.Controls.Add(this.btnInvoke);
            this.Controls.Add(this.txbData);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.btnAddToFirewall);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Button btnRedisSet;
        private System.Windows.Forms.Button btnDotNetty;
        private System.Windows.Forms.Button btnDalQuery;
        private System.Windows.Forms.Button btnFTS;
        private System.Windows.Forms.Button btnSTF;
        private System.Windows.Forms.Button btnSDKTesting;
        private System.Windows.Forms.Button btnChangeType;
        private System.Windows.Forms.Button btnsql;
        private System.Windows.Forms.Button btnDynamic;
        private System.Windows.Forms.Button btnZipPackage;
        private System.Windows.Forms.Button btnJWTEncode;
        private System.Windows.Forms.TextBox txbHeader;
        private System.Windows.Forms.TextBox txbPayload;
        private System.Windows.Forms.TextBox txbSecret;
        private System.Windows.Forms.Button btnJWTDecode;
        private System.Windows.Forms.Button btnLogAsync;
        private System.Windows.Forms.Button btnLogSync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCpu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalMem;
        private System.Windows.Forms.Label lblUsedMem;
        private System.Windows.Forms.Button btnSms;
        private System.Windows.Forms.Button btnProxy;
        private System.Windows.Forms.Button btnDalTest;
    }
}

