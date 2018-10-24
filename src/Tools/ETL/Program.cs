using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Bzway.Tools.ETL
{
    class Program
    {

        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                var i = 0;
                while (true)
                {
                    Console.WriteLine("test"+i.ToString());
                    Thread.Sleep(1000);
                    i++;
                }
            });

            Console.ReadKey();
            return;
            //foreach (var file in System.IO.Directory.GetFiles(@"D:\Desktop\COTY CB\LLB data till 201806"))
            //{
            //    var content = File.ReadAllLines(file,Encoding.UTF8);
            //    var headerLength = content[0].Split(";").Length;
            //    StringBuilder sbError = new StringBuilder();
            //    StringBuilder sb = new StringBuilder();
            //    foreach (var line in content)
            //    {
            //        var items = line.Split(";");
            //        if (items.Length != headerLength)
            //        {
            //            sbError.AppendLine(line);
            //        }
            //        sb.AppendLine(  string.Join(',', items.Select(m =>   m.Trim('"').Replace(",", "&nlno")  )));
            //    }
            //    sb.AppendLine(sbError.ToString());
            //    File.WriteAllText( file +".txt"  , sb.ToString(), Encoding.UTF8);
            //}

            foreach (var item in args)
            {
                ETL.Instance.Do(item);
            }
        }
    }
}