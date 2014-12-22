using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Monitor.Lib
{
    public class FTPUrl : BaseFTPUrl
    {
        public override string Prefix
        {
            get
            {
                return "ftp";
            }
        }

        private int port = 21;
        public override int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
