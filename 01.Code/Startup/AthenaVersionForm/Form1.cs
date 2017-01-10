using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SOAFramework.Library;
using SOAFramework.Library.DAL;

namespace AthenaVersionForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = txbPath.Text;
            var type = txbType.Text;
            var prefix = txbPrefix.Text;
            var version = "1.0.0.0";
            DirectoryInfo directory = new DirectoryInfo(path);
            var files = directory.GetFiles("*.*", SearchOption.AllDirectories);
            IDBHelper helper = DBFactory.CreateDBHelper(DBType.Oracle);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var name = file.FullName.Replace(path, "").TrimStart('\\').Replace("modules", "").TrimStart('\\');
                var id = string.Format("{0}_{1}", prefix, i);
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO TAB_FILEVERSION (FID,FILENAME,VERSION,LASTUPDATEDATE,SYSTYPE) VALUES(:FID,:FILENAME,:VERSION,:LASTUPDATETIME,:SYSTYPE)");
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter { Name = ":FID", Value = id });
                parameters.Add(new Parameter { Name = ":FILENAME", Value = name });
                parameters.Add(new Parameter { Name = ":VERSION", Value = version });
                parameters.Add(new Parameter { Name = ":LASTUPDATETIME", Value = DateTime.Now });
                parameters.Add(new Parameter { Name = ":SYSTYPE", Value = type });
                helper.ExecNoneQueryWithSQL(sql.ToString(), parameters.ToArray());
            }

            MessageBox.Show("上传完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["test1"] = "hello";
            dic["test2"] = 2;
            string str = XMLHelper.Serialize(dic);
        }
    }
}
