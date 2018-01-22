using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bzway.Database.Core;
using NPOI.XSSF.UserModel;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using Bzway.Tools.Grok.Core;
using System.Text.RegularExpressions;

namespace Bzway.Tools.ETL
{
    public class SourceReader
    {
        public static string MD5(string content)
        {

            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            byte[] bs = Encoding.UTF8.GetBytes(content);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                bs = md5.ComputeHash(bs);
            };
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bs)
            {
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            content = stringBuilder.ToString();
            return content;
        }
        public static List<DynamicEntity> ReadCsv(string path, char span = ',', int maxRecord = 100)
        {
            List<DynamicEntity> list = new List<DynamicEntity>();

            try
            {
                var positionFile = Path.Combine(AppContext.BaseDirectory, string.Format("{0}.{1}", MD5(path), "position"));
                int position = 0;
                if (File.Exists(positionFile))
                {
                    position = int.Parse(File.ReadAllText(positionFile));
                }
                using (var fileReader = new StreamReader(File.OpenRead(path)))
                {
                    var header = fileReader.ReadLine().Split(span);
                    if (fileReader.BaseStream.Length == position)
                    {
                        return list;
                    }
                    if (fileReader.BaseStream.Position < position)
                    {
                        fileReader.BaseStream.Position = position;
                    }
                    while (fileReader.EndOfStream == false && maxRecord > 0)
                    {
                        var body = fileReader.ReadLine().Split(span);
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var item in header)
                        {
                            if (body.Length - 1 < dict.Count)
                            {
                                dict.Add(item, null);
                            }
                            else
                            {
                                dict.Add(item, body[dict.Count]);
                            }
                        }
                        DynamicEntity entity = new DynamicEntity(dict);
                        list.Add(entity);
                        File.WriteAllText(positionFile, fileReader.BaseStream.Position.ToString());
                        maxRecord--;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadCsv Error:" + ex.Message);
            }
            return list;
        }
        public static List<DynamicEntity> ReadLog(string path, int maxRecord = 100)
        {
            List<DynamicEntity> list = new List<DynamicEntity>();

            try
            {
                var positionFile = Path.Combine(AppContext.BaseDirectory, string.Format("{0}.{1}", MD5(path), "position"));
                int position = 0;
                if (File.Exists(positionFile))
                {
                    position = int.Parse(File.ReadAllText(positionFile));
                }
                using (var fileReader = new StreamReader(File.OpenRead(path)))
                {
                    if (fileReader.BaseStream.Length == position)
                    {
                        return list;
                    }
                    if (fileReader.BaseStream.Position < position)
                    {
                        fileReader.BaseStream.Position = position;
                    }
                    while (fileReader.EndOfStream == false && maxRecord > 0)
                    {
                        var input = fileReader.ReadLine();
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("message", input);
                        DynamicEntity entity = new DynamicEntity(dict);
                        list.Add(entity);
                        File.WriteAllText(positionFile, fileReader.BaseStream.Position.ToString());
                        maxRecord--;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadLog Error:" + ex.Message);
            }
            return list;
        }
        public static List<DynamicEntity> ReadExcel(string path)
        {
            List<DynamicEntity> list = new List<DynamicEntity>();
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook(path);
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    var header = sheet.GetRow(0).Cells.Select(m => m.ToString()).ToArray();
                    var positionFile = Path.Combine(AppContext.BaseDirectory, string.Format("{0}.{2}.{1}", MD5(path), "position", sheet.SheetName));
                    int position = 1;
                    if (File.Exists(positionFile))
                    {
                        position = int.Parse(File.ReadAllText(positionFile));
                    }
                    //遍历每一行数据
                    for (int j = position; j < sheet.PhysicalNumberOfRows; j++)
                    {
                        var body = sheet.GetRow(j);
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var item in header)
                        {
                            dict.Add(item, body.Cells[dict.Count].ToString());
                        }
                        DynamicEntity entity = new DynamicEntity(dict);
                        list.Add(entity);
                        File.WriteAllText(positionFile, j.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadExcel Error:" + ex.Message);
            }
            return list;
        }
        public static List<DynamicEntity> ReadSQLServer(string connectionString, string cmd, int maxRecord = 1000)
        {
            List<DynamicEntity> list = new List<DynamicEntity>();
            var positionFile = string.Format("{0}.{1}", MD5(connectionString + cmd), "position");
            int position = 1;
            if (File.Exists(positionFile))
            {
                position = int.Parse(File.ReadAllText(positionFile));
            }
            try
            {
                var sql = string.Format("select * from ({0}) a where a.id>=@From and a.Id<=@To", cmd);
                using (var connection = new SqlConnection(connectionString))
                {
                    foreach (var dict in connection.Query(sql, new { From = position, To = position + maxRecord }).Select(x => x as IDictionary<string, object>))
                    {
                        DynamicEntity entity = new DynamicEntity(dict);
                        list.Add(entity);
                    }
                    position += maxRecord;
                    File.WriteAllText(positionFile, position.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadSQLServer Error:" + ex.Message);
            }
            return list;
        }
    }
}