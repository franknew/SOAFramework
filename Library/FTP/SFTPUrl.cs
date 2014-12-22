using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Monitor.Lib
{
    public class SFTPUrl : BaseFTPUrl
    {

        public override string Prefix
        {
            get
            {
                return "sftp";
            }
        }

        private int port = 22;
        public override int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
