using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Reflection;
using System.Collections;

namespace SOAFramework.Library
{
    public class TxtHelper
    {
        private const string _symbol = ",";

        /// <summary>
        /// 导出到txt文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="table"></param>
        /// <param name="showColumnName"></param>
        public static void DataTableToTxt(string fullFileName, DataTable table, bool showColumnName = false)
        {
            FileInfo file = new FileInfo(fullFileName);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            StringBuilder txt = new StringBuilder();
            StringBuilder txtTemp = new StringBuilder();
            Dictionary<string, int> lengthDic = GetLengthDic();
            if (showColumnName)
            {
                foreach (DataColumn column in table.Columns)
                {
                    txtTemp.Append(column.ColumnName);
                    if (lengthDic.ContainsKey(column.ColumnName))
                    {
                        int length = lengthDic[column.ColumnName];
                        if (length > column.ColumnName.GetChineseLength())
                        {
                            txtTemp.Append(new string(' ', length - column.ColumnName.GetChineseLength()));
                        }
                    }
                    txtTemp.Append(_symbol);
                }
                txt.AppendLine(txtTemp.ToString().TrimEnd(new char[] { ',' }));
                txt.AppendLine();
            }
            foreach (DataRow row in table.Rows)
            {
                txtTemp = new StringBuilder();
                foreach (DataColumn column in table.Columns)
                {
                    txtTemp.Append(row[column.ColumnName].ToString());
                    if (lengthDic.ContainsKey(column.ColumnName))
                    {
                        int length = lengthDic[column.ColumnName];
                        int chineseLength = row[column.ColumnName].ToString().GetChineseLength();
                        if (length > chineseLength)
                        {
                            txtTemp.Append(new string(' ', length - chineseLength));
                        }
                    }
                    txtTemp.Append(_symbol);
                }
                txt.AppendLine(txtTemp.ToString().TrimEnd(new char[] { ',' }));
                txt.AppendLine();
            }
            File.WriteAllText(fullFileName, txt.ToString());
        }

        public static void ObjectListToTxt(string fullFileName, IList list, bool showColumnName = false)
        {
            FileInfo file = new FileInfo(fullFileName);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            StringBuilder txt = new StringBuilder();
            Type t = null;
            if (list != null && list.Count > 0)
            {
                t = list[0].GetType();
            }
            if (showColumnName && t != null)
            {
                PropertyInfo[] properties = t.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    txt.Append(property.Name).Append(_symbol);
                }
                if (txt.Length > 0)
                {
                    txt.Remove(txt.Length - _symbol.Length, _symbol.Length);
                }
                txt.AppendLine();
            }
            foreach (var data in list)
            {
                PropertyInfo[] properties = t.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    txt.Append(property.GetValue(data, null).ToString()).Append(_symbol);
                }
                if (txt.Length > 0)
                {
                    txt.Remove(txt.Length - _symbol.Length, _symbol.Length);
                }
                txt.AppendLine();
            }
            File.WriteAllText(fullFileName, txt.ToString());
        }

        private static Dictionary<string, int> GetLengthDic()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic["BarData_网点编号"] = 14;
            dic["BarData_网点名称"] = 18;
            dic["RY_员工编号"] = 14;
            dic["RY_姓名"] = 10;
            dic["USER_员工编号"] = 9;
            dic["USER_密码"] = 9;
            dic["ZD_网点编号"] = 14;
            dic["ZD_网点名称"] = 20;
            return dic;
        }
    }
}
