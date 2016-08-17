using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;

namespace NPOI
{
    class Program
    {
        static void Main(string[] args)
        {

            // Create a new DataTable.
            System.Data.DataTable table = new DataTable("ParentTable");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ParentItem";
            column.AutoIncrement = false;
            column.Caption = "ParentItem";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["id"];
            table.PrimaryKey = PrimaryKeyColumns;

            // Create three new DataRow objects and add 
            // them to the DataTable
            for (int i = 0; i <= 2; i++)
            {
                row = table.NewRow();
                row["id"] = i;
                row["ParentItem"] = "ParentItem " + i;
                table.Rows.Add(row);
            }


            string excelName = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "Data.xls";
            string xpath = @"E:/GitRepo/NPOI/" + excelName;

            XlsCreate(table, xpath);
            XlsOpen(table, xpath);
        }


        static void XlsCreate(DataTable dt, string xpath)
        {
            //创建工作薄
            HSSFWorkbook workbook = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet isheet = workbook.CreateSheet("sheet1");

            int irow = dt.Rows.Count;
            int col = dt.Columns.Count;
            for (int i = 0; i < irow; i++)
            {
                IRow detailRow = isheet.CreateRow(i + 1);
                for (int j = 0; j < col; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    detailRow.CreateCell(j, CellType.String).SetCellValue(str);
                }
            }

            //add header
            ICellStyle hstyle = (ICellStyle)workbook.CreateCellStyle();
            hstyle.FillPattern = FillPattern.SolidForeground;
            hstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
            int size = dt.Columns.Count;
            IRow headerRow = isheet.CreateRow(0);
            for (int i = 0; i < size; i++)
            {
                string strCol = dt.Columns[i].ColumnName;
                ICell hcell = headerRow.CreateCell(i, CellType.String);
                hcell.SetCellValue(strCol);
                hcell.CellStyle = hstyle;

            }

            using (FileStream fs = File.OpenWrite(xpath)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            {
                workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
            }
        }

        static void XlsOpen(DataTable table, string xpath)
        {
            using (var fs = File.Open(xpath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                if (xpath.Substring(xpath.LastIndexOf('.')+1) == "xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else if (xpath.Substring(xpath.LastIndexOf('.')+1) == "xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    return ;
                }

                ISheet sheet = workbook.CreateSheet("sheet2");
                // Create detail excel sheet row

                int irow = table.Rows.Count;
                int col = table.Columns.Count;
                for (int i = 0; i < irow; i++)
                {
                    IRow detailRow = sheet.CreateRow(i + 1);
                    for (int j = 0; j < col; j++)
                    {
                        string str = table.Rows[i][j].ToString();
                        detailRow.CreateCell(j, CellType.String).SetCellValue(str);
                    }
                }

                FileStream file = new FileStream(xpath, FileMode.Create);
                workbook.Write(file);
                if (file.CanWrite)
                {
                    file.Flush();
                    file.Close();
                }
                file.Dispose();

            }

        }
    }
}
