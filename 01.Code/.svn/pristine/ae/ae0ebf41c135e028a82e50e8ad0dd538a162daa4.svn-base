using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceCore.Models
{
    public class ComputerInfo
    {
        public string MachineName = null;
        public string MacAddress = null;
        public string IP = null;
        public string OSName = null;
        public string OSVersion = null;

        public ComputerInfo()
        {
            MachineName = Environment.MachineName;
            OSName = Environment.OSVersion.Platform.ToString();
            OSVersion = Environment.OSVersion.VersionString;
        }
    }
}
