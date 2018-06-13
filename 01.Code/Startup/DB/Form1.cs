using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSQL_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            helper.ExecNoneQueryWithSQL(txbSQL.Text);
            MessageBox.Show("ok");
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            DataTable table = helper.GetTableWithSQL(txbSQL.Text);

            MessageBox.Show(JsonHelper.Serialize(table));
        }
    }
}
