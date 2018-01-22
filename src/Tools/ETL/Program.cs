using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Bzway.Tools.ETL
{
    class Program
    {

        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                ETL.Instance.Do(item);
            }
        }
    }
}