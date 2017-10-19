using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SOAFramework.Library
{
    delegate object SyncControlPropertyGetter(Control control, string property = "Text");

    delegate void SyncControlPropertySetter(Control control, object value, string property = "Text");

    public class UISync
    {
        public static void Invoke(ISynchronizeInvoke sync, Action action)
        {
            if (!sync.InvokeRequired)
            {
                action();
            }
            else
            {
                object[] args = new object[] { };
                sync.Invoke(action, args);
            }
        }
        public static void BeginInvoke(ISynchronizeInvoke sync, Action action)
        {

            sync.BeginInvoke(action, null);

        }

        #region 跨线程访问属性

        public static object GetUIValue( Control control, string property = "Text")
        {
            if (control.InvokeRequired)
            {
                SyncControlPropertyGetter getter = new SyncControlPropertyGetter(InnerGetUIValue);
                var form = control.FindForm();
                return form.Invoke(getter, new object[] { control, property });
            }
            else return InnerGetUIValue(control, property);
        }

        public static void SetUIValue(Control control, object value, string property = "Value")
        {
            if (control.InvokeRequired)
            {
                SyncControlPropertySetter setter = new SyncControlPropertySetter(InnerSetUIValue);
                var form = control.FindForm();
                form.Invoke(setter, new object[] { control, value, property });
            }
            else InnerSetUIValue(control, property);
        }

        private static object InnerGetUIValue(Control control, string property = "Text")
        {
            PropertyInfo propertyInfo = control.GetType().GetProperty(property);
            return propertyInfo.GetValue(control, null);
        }

        private static void InnerSetUIValue(Control control, object value, string property = "Value")
        {
            PropertyInfo propertyInfo = control.GetType().GetProperty(property);
            propertyInfo.SetValue(control, value, null);
        }

        #endregion
    }

}
