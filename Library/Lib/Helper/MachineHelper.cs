using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace SOAFramework.Library
{
    public class MachineHelper
    {
        /// <summary>
        /// 获得本机MAC
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        } 
        
        /// <summary>
        /// CMD命令ping服务器地址
        /// </summary>
        /// <param name="_strHost"></param>
        /// <returns></returns>
        public static bool CmdPing(string _strHost)
        {
            bool valid = true;
            string m_strHost = _strHost;

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            string pingrst = string.Empty;
            process.StartInfo.Arguments = "ping   " + m_strHost + "   -n   1";
            process.Start();
            process.StandardInput.AutoFlush = true;
            string temp = "ping   " + m_strHost + "   -n   1";
            process.StandardInput.WriteLine(temp);
            process.StandardInput.WriteLine("exit");
            string strRst = process.StandardOutput.ReadToEnd();
            if (strRst.IndexOf("(0%   loss)") != -1)
            {
                pingrst = "连接";
                valid = false;
            }
            else if (strRst.IndexOf("Destination   host   unreachable.") != -1)
            {
                pingrst = "无法到达目的主机";
                valid = false;
            }
            else if (strRst.IndexOf("Request   timed   out.") != -1)
            {
                pingrst = "超时";
                valid = false;
            }
            else if (strRst.IndexOf("Unknown   host") != -1)
            {
                pingrst = "无法解析主机";
                valid = false;
            }
            else
            {
                pingrst = strRst;
            }
            process.Close();
            return valid;
        }
    }
}
