
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SOAFramework.Library
{
    public static class ControlExtension
    {
        public static void SetUIValue(this Control control, object value, string property = "Value")
        {
            UISync.SetUIValue(control, value, property);
        }

        public static object GetUIValue(this Control control, string property = "Text")
        {
            return UISync.GetUIValue(control, property);
        }
    }
}