using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace SOAFramework.Common
{
    public class MachineUtil
    {
        public static string GetLocalMacAddress()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            moc = mc.GetInstances();
            string strMacAddress = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    strMacAddress = mo["MacAddress"].ToString();
            }
            return strMacAddress;  
        }
    }
}
