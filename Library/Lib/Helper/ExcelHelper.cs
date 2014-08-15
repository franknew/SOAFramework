using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.IO;
using NPOI.HSSF.Util;
using System.Data.OleDb;

namespace Athena.Unitop.Sure.Lib
{
    public class ExecelHelper3
    {

        /// <summary>
        /// NPOI简单Demo，快速入门代码
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="strFileName"></param>
        /// <remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks>
        /// <Author>柳永法 http://www.yongfa365.com/ 2010-5-8 22:21:41</Author>
        public static void ExportEasy(DataTable dtSource, string strFileName)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            foreach (DataColumn column in dtSource.Columns)
            {
                dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }


            //填充内容
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
                }
            }


            //保存
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }

        }
    }
    /// <summary>
    /// 使用开源框架NPOI导出到EXCEL
    /// </summary>
    public class ExcelHelper
    {
        private static Dictionary<String, ICellStyle> Styles = new Dictionary<string, ICellStyle>();

        //public static DataSet FromExcel(string fileName)
        //{
        //    DataSet ds;
        //    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;data source=" + fileName;
        //    OleDbConnection myConn = new OleDbConnection(strCon);
        //    //string strCom = TSql;
        //    myConn.Open();
        //    OleDbDataAdapter myCommand = new OleDbDataAdapter("", myConn);
        //    ds = new DataSet();
        //    myCommand.Fill(ds);
        //    myConn.Close();
        //    return ds;
        //}

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static DataSet ReadExcel(string fullFileName)
        {
            int count = 0;
            int index = 0;
            return ReadExcel(fullFileName, ref count, ref index);
        }

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static DataSet ReadExcel(string fullFileName, ref int recordCount, ref int currentIndex)
        {
            DataSet dsData = new DataSet();
            currentIndex = 0;
            using (FileStream fs = new FileStream(fullFileName, FileMode.Open))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                int sheetCount = workbook.NumberOfSheets;
                recordCount = 0;
                //计算数据总量
                for (int i = 0; i < sheetCount; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    recordCount += sheet.PhysicalNumberOfRows;
                }
                for (int i = 0; i < sheetCount; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    IRow header = sheet.GetRow(0);
                    DataTable dtData = new DataTable();
                    if (header == null || header.Cells == null)
                    {
                        break;
                    }
                    foreach (ICell cell in header.Cells)
                    {
                        if (!dtData.Columns.Contains(cell.StringCellValue))
                        {
                            dtData.Columns.Add(cell.StringCellValue);
                        }
                    }
                    int rowCount = sheet.PhysicalNumberOfRows;
                    for (int x = 1; x < rowCount; x++)
                    {
                        IRow row = sheet.GetRow(x);
                        DataRow newRow = dtData.NewRow();
                        foreach (ICell cell in row.Cells)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.STRING:
                                    newRow[cell.ColumnIndex] = cell.StringCellValue;
                                    break;
                                case CellType.BOOLEAN:
                                    newRow[cell.ColumnIndex] = cell.BooleanCellValue;
                                    break;
                                case CellType.NUMERIC:
                                case CellType.FORMULA:
                                    newRow[cell.ColumnIndex] = cell.NumericCellValue;
                                    break;
                                case CellType.Unknown:
                                    newRow[cell.ColumnIndex] = cell.StringCellValue;
                                    break;
                            }
                        }
                        dtData.Rows.Add(newRow);
                        currentIndex++;
                    }
                    dsData.Tables.Add(dtData);
                }
            }
            return dsData;
        }

        private static void InitStyle(IWorkbook hssfworkbook)
        {

            ICellStyle title = hssfworkbook.CreateCellStyle();
            title.Alignment = HorizontalAlignment.CENTER;
            IFont font1 = hssfworkbook.CreateFont();
            font1.Color = HSSFColor.BLUE.index;
            font1.Boldweight = (short)FontBoldWeight.BOLD;
            font1.FontHeightInPoints = 20;

            title.SetFont(font1);

            Styles.Add("title", title);

            ICellStyle summary = hssfworkbook.CreateCellStyle();
            summary.Alignment = HorizontalAlignment.CENTER;

            summary.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE.index;
            summary.FillPattern = FillPatternType.SOLID_FOREGROUND;
            //create a font style
            IFont font = hssfworkbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.BOLD;
            summary.SetFont(font);

            Styles.Add("summary", summary);

            ICellStyle header = hssfworkbook.CreateCellStyle();
            SetBorder(header);
            Styles.Add("header", header);

            ICellStyle cell = hssfworkbook.CreateCellStyle();
            SetBorder(cell);
            Styles.Add("cell", cell);
            ICellStyle date = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            date.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
            SetBorder(date);
            Styles.Add("date", date);
        }


        public static string ToExcelFile(DataTable table, string filePath, string fileName, ref int processIndex, string title = null,
          List<string> computeColumns = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            if (string.IsNullOrEmpty(table.TableName)) table.TableName = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
            DataSet ds = new DataSet(table.TableName);
            ds.Tables.Add(table);
            return ToExcelFile(ds, filePath, fileName, ref processIndex, title, computeColumns, version);
        }

        public static string ToExcelFile(DataSet ds, string filePath, string fileName, ref int processIndex, string title = null,
         List<string> computeColumns = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            StringBuilder fullFileName = new StringBuilder();
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            fullFileName.Append(filePath.Trim('\\')).Append("\\").Append(fileName);
            IWorkbook hssfworkbook = null;
            if (version == ExcelVersion.Excel2007)
            {
                hssfworkbook = new XSSFWorkbook();
                fullFileName.Append(".xlsx");
            }
            else
            {
                hssfworkbook = new HSSFWorkbook();
                fullFileName.Append(".xls");
            }
            InitStyle(hssfworkbook);
            foreach (DataTable table in ds.Tables)
            {
                if (string.IsNullOrEmpty(table.TableName)) table.TableName = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
                ISheet sheet = hssfworkbook.CreateSheet(table.TableName);
                int index = 0;
                int rowStartIndex = index;
                //处理title
                if (!string.IsNullOrEmpty(title))
                {
                    index = 1;
                    CreateTitle(title, table.Columns.Count, sheet);

                }
                //处理header
                CreateHeader(table, sheet, ref index);
                rowStartIndex = index;
                //处理数据
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    CreateRow(table.Rows[i], sheet, index);
                    index++;
                    processIndex++;
                }
                //处理汇总
                if (computeColumns != null && computeColumns.Count > 0)
                {
                    IRow summaryRow = sheet.CreateRow(index);


                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        ICell cell = summaryRow.CreateCell(i);
                        if (computeColumns.Contains(table.Columns[i].ColumnName))
                        {
                            StringBuilder sbExpression = new StringBuilder();
                            sbExpression.Append("SUM(").Append(GetColumnReference(i)).Append((rowStartIndex + 1).ToString());
                            sbExpression.Append(":").Append(GetColumnReference(i)).Append((index).ToString()).Append(")");

                            cell.CellFormula = sbExpression.ToString();
                        }
                        cell.CellStyle = Styles["summary"];
                    }
                }

            }

            FileStream file = new FileStream(fullFileName.ToString(), FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            Styles.Clear();
            return fullFileName.ToString();
        }
        /// <summary>
        /// 获得行号（转换成字母）
        /// </summary>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static string GetColumnReference(int colIndex)
        {
            int dividend = colIndex + 1;
            string columnName = String.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (int)((dividend - modifier) / 26);
            }
            return columnName;
        }
        private static void SetBorder(ICellStyle style)
        {
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
        }

        private static void CreateRow(DataRow dataRow, ISheet sheet, int startIndex)
        {
            IRow row = sheet.CreateRow(startIndex);
            int count = dataRow.Table.Columns.Count;

            for (int i = 0; i < count; i++)
            {
                ICell cell = row.CreateCell(i);
                DataColumn column = dataRow.Table.Columns[i];
                string drValue = dataRow[column].ToString();

                switch (column.DataType.ToString())
                {
                    case "System.String": //字符串类型
                        cell.SetCellValue(drValue);
                        cell.CellStyle = Styles["cell"];
                        break;
                    case "System.DateTime": //日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);

                        cell.SetCellValue(dateV);
                        cell.CellStyle = Styles["date"]; //格式化显示
                        break;
                    case "System.Boolean": //布尔型
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        cell.SetCellValue(boolV);
                        cell.CellStyle = Styles["cell"];
                        break;
                    case "System.Int16": //整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        cell.SetCellValue(intV);
                        cell.CellStyle = Styles["cell"];
                        break;
                    case "System.Decimal": //浮点型
                    case "System.Double":
                    case "System.Single":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        cell.SetCellValue(doubV);
                        cell.CellStyle = Styles["cell"];
                        break;
                    case "System.DBNull": //空值处理
                        cell.SetCellValue("");
                        cell.CellStyle = Styles["cell"];
                        break;
                    default:
                        cell.SetCellValue(drValue);
                        cell.CellStyle = Styles["cell"];
                        break;

                }


            }
            startIndex++;
        }

        private static void CreateHeader(DataTable table, ISheet sheet, ref int startIndex)
        {
            IRow row = sheet.CreateRow(startIndex);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn col = table.Columns[i];
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(col.ColumnName);
                cell.CellStyle = Styles["header"];
            }
            startIndex++;
        }

        /// <summary>
        /// 创建标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="hssfworkbook"></param>
        /// <param name="table"></param>
        /// <param name="sheet"></param>
        private static void CreateTitle(string title, int columnCount, ISheet sheet)
        {
            IRow row = sheet.CreateRow(0);
            row.HeightInPoints = 30;

            ICell cell = row.CreateCell(0);
            //set the title of the sheet
            cell.SetCellValue(title);


            cell.CellStyle = Styles["title"];

            //merged cells on single row
            //ATTENTION: don't use Region class, which is obsolete
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, columnCount - 1));
        }

        #region 模版输出 -陈超超

        /// <summary>
        /// 以实体作为数据源创建模版
        /// </summary>
        /// <param name="excelTemplate">模版路径</param>
        /// <param name="outPath">输出路径</param>
        /// <param name="model">输入模型，替换方式采用RazorEngine引擎解析，如“@(Model.A==1?"是":"否")”</param>
        /// <param name="rowsCount">行替换范围，默认遇到Null时停止</param>
        /// <param name="columnsCount">列替换范围，默认遇到Null时停止</param>
        public static void ToExcelTemplate<T>(string excelTemplate, string outPath, T model, int rowsCount = -1, int columnsCount = -1)
        {
            ExcelParse(excelTemplate, outPath, (x) =>
            {
                string razorTemplaete = "@" + x.Substring(1);
                try
                {
                    string rel = Athena.Unitop.Sure.Lib.RazorEngine.Razor.Parse<T>(razorTemplaete, model);
                    return rel;

                }
                catch (Athena.Unitop.Sure.Lib.RazorEngine.Templating.TemplateCompilationException ex)
                {

                    foreach (var item in ex.Errors)
                        System.Diagnostics.Debug.Print("===> " + ex.Message);
                    return "";
                }
                catch (Exception ex)
                {
                    throw;
                }
            }, rowsCount, columnsCount);
        }

        /// <summary>
        /// 以字典作为数据源创建模版
        /// </summary>
        /// <param name="excelTemplate">模版路径</param>
        /// <param name="outPath">输出路径</param>
        /// <param name="model">输入字典,替换方式采用一一对应替换，如“#编号”</param>
        /// <param name="rowsCount">行替换范围，默认遇到Null时停止</param>
        /// <param name="columnsCount">列替换范围，默认遇到Null时停止</param>
        public static void ToExcelTemplate(string excelTemplate, string outPath, Dictionary<string, string> model, int rowsCount=-1, int columnsCount=-1)
        {
            ExcelParse(excelTemplate, outPath, (x) =>
            {
                if (model.ContainsKey(x))
                    return model[x];
                else
                    return null;
            }, rowsCount, columnsCount);
        }

        /// <summary>
        /// 表格作为数据源创建模版
        /// </summary>
        /// <param name="excelTemplate">模版路径</param>
        /// <param name="outPath">输出路径</param>
        /// <param name="model">输入表格，替换方式采用简单公式，如“#字段.行号”</param>
        /// <param name="rowsCount">行替换范围，默认遇到Null时停止</param>
        /// <param name="columnsCount">列替换范围，默认遇到Null时停止</param>
        public static void ToExcelTemplate(string excelTemplate, string outPath, DataTable model, int rowsCount = -1, int columnsCount = -1)
        {
            ExcelParse(excelTemplate, outPath, (x) =>
            {
                //先获得表的填充坐标
                string[] s = x.Substring(1).Split('.');
                string colName = "";
                int rowIndex = 0;
                if (s.Length == 0) return "";
                else if (s.Length == 1) { colName = s[0]; }
                else { colName = s[0]; int.TryParse(s[1], out rowIndex); }

                //返回数据
                if (rowIndex >= model.Rows.Count) return "";
                if (model.Columns.Contains(colName) == false) return "";

                var value = model.Rows[rowIndex][colName];
                if (value.GetType() == typeof(System.DBNull)) return "";
                return value.ToString();
            }, rowsCount, columnsCount);
        }

        private static void ExcelParse(string excelTemplate, string outPath, Func<string, string> GetValue, int rowsCount = -1, int columnsCount = -1)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelTemplate);
            ISheet sheet = workbook.GetSheetAt(0);

            int rowIndex = 0;
            IRow row = sheet.GetRow(rowIndex);
            while ((row != null && rowsCount == -1) || (rowIndex < rowsCount && rowsCount != -1))
            {
                if (row != null)
                {
                    int columnIndex = 0;
                    ICell cell = row.GetCell(columnIndex);
                    while ((cell != null && columnsCount == -1) || (columnIndex < columnsCount && columnsCount != -1))
                    {
                        if (cell != null)
                        {
                            if (cell.CellType == CellType.STRING && cell.StringCellValue.Length > 1 && cell.StringCellValue[0] == '#')
                            {
                                string value = GetValue(cell.StringCellValue);
                                if (value != null) cell.SetCellValue(value);
                            }
                        }
                        columnIndex++;
                        cell = row.GetCell(columnIndex);
                    }
                }
                rowIndex++;
                row = sheet.GetRow(rowIndex);
            }
            workbook.Write(new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.Write));
        }

        #endregion

    }
}
