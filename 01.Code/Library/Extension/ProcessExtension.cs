using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public static class ProcessExtension
    {
        public static void SetProcessName(this Process process, string name)
        {
            WindowsApi.SetWindowText(process.MainWindowHandle, name);
        }
    }
}
