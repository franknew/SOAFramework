using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinformTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProxyFactory proxy = new ProxyFactory(new Form1());
            proxy.AddAdvice(new EventAdvise());
            Application.Run(new Form1());
            //IFormAction i = (IFormAction)proxy.GetProxy();
            //var form = i as Form1;
            //Application.Run(form);
        }

      
    }

    public interface IFormAction
    {
        void OnClick(EventArgs e);
    }
}
