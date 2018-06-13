using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class Performance
    {
        static readonly PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static readonly PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available Bytes");

        public PerformanceCounter CpuCounter => cpuCounter;
        private bool isFirst = true;

        public Performance()
        {
        }
        
        public float GetCurrentCpuUsage()
        {
            return CpuCounter.NextValue();
        }

        public long GetAvailableRamSize()
        {
            return (long)ramCounter.NextValue();
        }

        public long GetTotalMem()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            long memory = 0;
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    memory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                    break;
                }
            }
            return memory;
        }

        public int GetCpuCount()
        {
            return Environment.ProcessorCount;
        }

        public string GetMachineName()
        {
            return Dns.GetHostName();
        }
    }
}
