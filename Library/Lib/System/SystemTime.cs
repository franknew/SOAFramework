using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Athena.Unitop.Sure.Lib
{
    [StructLayout(LayoutKind.Sequential)]
    public class SystemTime
    {
        public ushort year;
        public ushort month;
        public ushort dayofweek;
        public ushort day;
        public ushort hour;
        public ushort minute;
        public ushort second;
        public ushort milliseconds;
    }


}
