using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinformTest
{
    public class EventAdvise: IThrowsAdvice
    {
        public void AfterThrowing(Exception ex)
        {
            MessageBox.Show("in throwing advice");
        }
    }
}
