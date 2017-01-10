using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Library
{
    public class NodeServerDataModel
    {
        public string PackageName { get; set; }
        public string Url { get; set; }

        public ServerStatusType Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        private ServerStatusType status = ServerStatusType.Wait;

        public string Error { get; set; }

    }

    public enum ServerStatusType
    {
        Wait,
        Started,
        Stoped,
        Close,
        Error,
    }
}
