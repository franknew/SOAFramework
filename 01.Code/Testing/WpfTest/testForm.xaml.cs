using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// testForm.xaml 的交互逻辑
    /// </summary>
    public partial class testForm : Window, IForm
    {
        public testForm()
        {
            InitializeComponent();
        }

        public void OnClick(EventArgs e)
        {
            base.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("need handle exception");
        }
    }
}
