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
    public partial class Host : Form
    {
        public Host()
        {
            InitializeComponent();
        }

        private MasterServer _server;

        private void button1_Click(object sender, EventArgs e)
        {
            if (_server == null) _server = new MasterServer();
            _server.DisplayShell = false;
            _server.StartNode(textBox1.Text);
        }
    }
}
