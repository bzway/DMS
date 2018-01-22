using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bzway.Server.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {


            using (var host = new HttpServer(9000))
            {

                host.Use(context =>
                {
                    return Task.Run(() =>
                    {
                        Console.WriteLine("Process...");
                        Thread.Sleep(100);
                        Console.WriteLine("Processed");
                    });
                });
                host.Start();
                Console.Read();
            }
        }
    }
}