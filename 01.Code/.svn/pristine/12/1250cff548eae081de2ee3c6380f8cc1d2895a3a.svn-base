using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SOAFramework.Library
{
    public class WinAPI
    {
        [DllImport("Kernel32.dll")]
        private static extern Boolean SetSystemTime([In, Out] SystemTime st);

        public static bool SetSysTime(DateTime newdatetime)
        {
            SystemTime st = new SystemTime();
            st.year = Convert.ToUInt16(newdatetime.Year);
            st.month = Convert.ToUInt16(newdatetime.Month);
            st.day = Convert.ToUInt16(newdatetime.Day);
            st.dayofweek = Convert.ToUInt16(newdatetime.DayOfWeek);
            st.hour = Convert.ToUInt16(newdatetime.Hour - TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(2001, 09, 01)).Hours);
            st.minute = Convert.ToUInt16(newdatetime.Minute);
            st.second = Convert.ToUInt16(newdatetime.Second);
            st.milliseconds = Convert.ToUInt16(newdatetime.Millisecond);
            return SetSystemTime(st);
        }
    }
}
