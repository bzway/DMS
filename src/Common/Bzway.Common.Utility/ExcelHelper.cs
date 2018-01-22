
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System.Text;

namespace Bzway.Common.Utility
{
    public class ExcelHelper
    {
        public static DataSet ReadCsv(string path, bool HeadYes = true, char span = ',')
        {
            DataTable dt = new DataTable();
            try
            {
                using (StreamReader fileReader = new StreamReader(path, Encoding.UTF8))
                {
                    //是否为第一行（如果HeadYes为TRUE，则第一行为标题行）
                    int lsi = 0;
                    //列之间的分隔符
                    char cv = span;
                    while (fileReader.EndOfStream == false)
                    {
                        string line = fileReader.ReadLine();
                        string[] y = line.Split(cv);
                        //第一行为标题行
                        if (HeadYes == true)
                        {
                            //第一行
                            if (lsi == 0)
                            {
                                for (int i = 0; i < y.Length; i++)
                                {
                                    dt.Columns.Add(y[i].Trim().ToString());
                                }
                                lsi++;
                            }
                            //从第二列开始为数据列
                            else
                            {
                                DataRow dr = dt.NewRow();
                                for (int i = 0; i < y.Length; i++)
                                {
                                    dr[i] = y[i].Trim();
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        //第一行不为标题行
                        else
                        {
                            if (lsi == 0)
                            {
                                for (int i = 0; i < y.Length; i++)
                                {
                                    dt.Columns.Add("Col" + i.ToString());
                                }
                                lsi++;
                            }
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < y.Length; i++)
                            {
                                dr[i] = y[i].Trim();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;

            }
            catch
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
        }
        public static DataSet ReadExcel(string fileNamePath)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(fileNamePath);
            DataSet dataSet = new DataSet();
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                DataTable dataTable = new DataTable(sheet.SheetName);
                foreach (var item in sheet.GetRow(0))
                {
                    dataTable.Columns.Add(item.ToString());
                }
                //遍历每一行数据
                for (int j = 1; j < sheet.PhysicalNumberOfRows; j++)
                {
                    var dataRow = dataTable.NewRow();

                    foreach (var cell in sheet.GetRow(j))
                    {
                        dataRow[cell.ColumnIndex] = cell.ToString();
                    }
                    dataTable.Rows.Add(dataRow);

                }
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

    }
}