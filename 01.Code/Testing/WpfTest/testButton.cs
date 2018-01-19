using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WinformTest;

namespace WpfTest
{
    public class testButton : Button, IButton
    {
        public testButton()
        {
            ProxyFactory factory = new ProxyFactory(this);
            factory.AddAdvice(new EventAdvise());
        }

        void IButton.OnClick()
        {
            base.OnClick();
        }
    }

    public interface IButton
    {
        void OnClick();
    }
}