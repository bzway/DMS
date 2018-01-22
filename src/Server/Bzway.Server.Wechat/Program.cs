using Bzway.Common.Collections;
using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bzway.Server.Wechat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.UseBaseProcess();
            server.UseMvc();
            //server.UseKeyWord();
            server.Run();
            Console.Read();
        }
    }
}