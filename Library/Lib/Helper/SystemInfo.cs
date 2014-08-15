using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Management;
using System.Text;

namespace Athena.Unitop.Sure.Lib
{
   
    /// <summary>
    /// SystemInfo 的摘要说明
    /// </summary>
    public class SystemInfo
    {
        private const int CHAR_COUNT = 128;
        public SystemInfo()
        {

        }
        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder WinDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder SysDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemInfo(ref CpuInfo cpuInfo);

        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MemoryInfo memInfo);

        [DllImport("kernel32")]
        private static extern void GetSystemTime(ref SystemTimeInfo sysInfo);

        /// <summary>
        /// 查询CPU编号
        /// </summary>
        /// <returns></returns>
        public string GetCpuId()
        {
            ManagementClass mClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mClass.GetInstances();
            string cpuId = null;
            foreach (ManagementObject mo in moc)
            {
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return cpuId;
        }

        /// <summary>
        /// 查询硬盘编号
        /// </summary>
        /// <returns></returns>
        public string GetMainHardDiskId()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            String hardDiskID = null;
            foreach (ManagementObject mo in searcher.Get())
            {
                hardDiskID = mo["SerialNumber"].ToString().Trim();
                break;
            }
            return hardDiskID;
        }

        /// <summary>
        /// 获取Windows目录
        /// </summary>
        /// <returns></returns>
        public string GetWinDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetWindowsDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取系统目录
        /// </summary>
        /// <returns></returns>
        public string GetSysDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetSystemDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        public CpuInfo GetCpuInfo()
        {
            CpuInfo cpuInfo = new CpuInfo();
            GetSystemInfo(ref cpuInfo);
            return cpuInfo;
        }

        /// <summary>
        /// 获取系统内存信息
        /// </summary>
        /// <returns></returns>
        public MemoryInfo GetMemoryInfo()
        {
            MemoryInfo memoryInfo = new MemoryInfo();
            GlobalMemoryStatus(ref memoryInfo);
            return memoryInfo;
        }

        /// <summary>
        /// 获取系统时间信息
        /// </summary>
        /// <returns></returns>
        public SystemTimeInfo GetSystemTimeInfo()
        {
            SystemTimeInfo systemTimeInfo = new SystemTimeInfo();
            GetSystemTime(ref systemTimeInfo);
            return systemTimeInfo;
        }

        /// <summary>
        /// 获取系统名称
        /// </summary>
        /// <returns></returns>
        public string GetOperationSystemInName()
        {
            OperatingSystem os = System.Environment.OSVersion;
            string osName = "UNKNOWN";
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0: osName = "Windows 95"; break;
                        case 10: osName = "Windows 98"; break;
                        case 90: osName = "Windows ME"; break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3: osName = "Windws NT 3.51"; break;
                        case 4: osName = "Windows NT 4"; break;
                        case 5: if (os.Version.Minor == 0)
                            {
                                osName = "Windows 2000";
                            }
                            else if (os.Version.Minor == 1)
                            {
                                osName = "Windows XP";
                            }
                            else if (os.Version.Minor == 2)
                            {
                                osName = "Windows Server 2003";
                            }
                            break;
                        case 6:
                            if (os.Version.Minor == 0)
                            {
                                osName = "WindowsVista";
                            }
                            else if (os.Version.Minor == 1)
                            {
                                osName = "Windows7";
                            }else
                            osName = "Windows8"; break;
                    }
                    break;
            }
            //return String.Format("{0},{1}", osName, os.Version.ToString());
            return osName;
        }
        public static void Show()
        {

            SystemInfo systemInfo = new SystemInfo();
            Console.WriteLine("操作系统：" + systemInfo.GetOperationSystemInName() + "<br>");
            Console.WriteLine("CPU编号：" + systemInfo.GetCpuId() + "<br>");
            Console.WriteLine("硬盘编号：" + systemInfo.GetMainHardDiskId() + "<br>");
            Console.WriteLine("Windows目录所在位置：" + systemInfo.GetSysDirectory() + "<br>");
            Console.WriteLine("系统目录所在位置：" + systemInfo.GetWinDirectory() + "<br>");
            MemoryInfo memoryInfo = systemInfo.GetMemoryInfo();
            CpuInfo cpuInfo = systemInfo.GetCpuInfo();
            Console.WriteLine("dwActiveProcessorMask" + cpuInfo.dwActiveProcessorMask + "<br>");
            Console.WriteLine("dwAllocationGranularity" + cpuInfo.dwAllocationGranularity + "<br>");
            Console.WriteLine("CPU个数：" + cpuInfo.dwNumberOfProcessors + "<br>");
            Console.WriteLine("OEM ID：" + cpuInfo.dwOemId + "<br>");
            Console.WriteLine("页面大小" + cpuInfo.dwPageSize + "<br>");
            Console.WriteLine("CPU等级" + cpuInfo.dwProcessorLevel + "<br>");
            Console.WriteLine("dwProcessorRevision" + cpuInfo.dwProcessorRevision + "<br>");
            Console.WriteLine("CPU类型" + cpuInfo.dwProcessorType + "<br>");
            Console.WriteLine("lpMaximumApplicationAddress" + cpuInfo.lpMaximumApplicationAddress + "<br>");
            Console.WriteLine("lpMinimumApplicationAddress" + cpuInfo.lpMinimumApplicationAddress + "<br>");
            Console.WriteLine("CPU类型：" + cpuInfo.dwProcessorType + "<br>");
            Console.WriteLine("可用交换文件大小：" + memoryInfo.dwAvailPageFile + "<br>");
            Console.WriteLine("可用物理内存大小：" + memoryInfo.dwAvailPhys + "<br>");
            Console.WriteLine("可用虚拟内存大小" + memoryInfo.dwAvailVirtual + "<br>");
            Console.WriteLine("操作系统位数：" + memoryInfo.dwLength + "<br>");
            Console.WriteLine("已经使用内存大小：" + memoryInfo.dwMemoryLoad + "<br>");
            Console.WriteLine("交换文件总大小：" + memoryInfo.dwTotalPageFile + "<br>");
            Console.WriteLine("总物理内存大小：" + memoryInfo.dwTotalPhys + "<br>");
            Console.WriteLine("总虚拟内存大小：" + memoryInfo.dwTotalVirtual + "<br>");
        }
        /// <summary>
        /// 可用物理内存大小
        /// </summary>
        /// <returns></returns>
        public string getPhysicalMemorySize()
        { 
             //PhysicalMemorySize 可用物理内存大小,与资源管理器中的关于对话框显示的内存大小一致
            //FreePhysicalMemory 剩余物理内存大小
            ulong PhysicalMemorySize = 0, VirtualMemorySize = 0, FreePhysicalMemory = 0;
            ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            foreach (ManagementObject obj in osClass.GetInstances())
            {
                if (obj["TotalVisibleMemorySize"] != null)
                    PhysicalMemorySize = (ulong)obj["TotalVisibleMemorySize"];

                if (obj["TotalVirtualMemorySize"] != null)
                    VirtualMemorySize = (ulong)obj["TotalVirtualMemorySize"];

                if (obj["FreePhysicalMemory"] != null)
                    FreePhysicalMemory = (ulong)obj["FreePhysicalMemory"];
                break;
            }
         
            if (PhysicalMemorySize > 0)
                return string.Format("{0:###,###,###} KB", PhysicalMemorySize);
            else
                return "Unknown";
        }
    }


}
