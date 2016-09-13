using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MicroService.Library;

namespace MicroServiceTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MasterServer server = new MasterServer();
            server.DisplayShell = false;
            server.StartNode(textBox1.Text);
        }
    }
}
