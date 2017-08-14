using MicroService.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace NodeInstance
{
    public partial class NodeService : ServiceBase
    {
        private MasterServer _server = new MasterServer();

        public NodeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _server.Start();
        }

        protected override void OnStop()
        {
            _server.CloseAllNode();
        }
    }
}
