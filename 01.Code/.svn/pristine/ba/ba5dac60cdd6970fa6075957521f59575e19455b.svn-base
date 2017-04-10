using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class Performance
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public Performance()
        {
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            try
            {
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch
            {

            }
        }

        public float GetCurrentCpuUsage()
        {
            return cpuCounter.NextValue();
        }

        public float GetCurrentRamUsage()
        {
            return ramCounter.NextValue();
        }
    }
}
