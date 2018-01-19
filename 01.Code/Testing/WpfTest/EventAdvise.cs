using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WinformTest
{
    public class EventAdvise: IThrowsAdvice
    {
        public void AfterThrowing(Exception ex)
        {
            Debug.Write(ex.Message);
        }
    }
}
