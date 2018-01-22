using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
namespace Bzway.Sites.BackOffice
{
    public class Program
    {
        const string pidFile = "web.pid";
        const string hostUrl = "http://localhost:9999";
        public static void Main(string[] args)
        {

            bool canRun = !File.Exists(pidFile);
            if (!canRun)
            {
                using (var stream = File.OpenText(pidFile))
                {
                    int pid = 0;
                    if (int.TryParse(stream.ReadLine(), out pid))
                    {
                        var process = Process.GetProcesses().FirstOrDefault(m => m.Id == pid);
                        canRun = (process == null);
                    }
                }
            }
            if (canRun)
            {
                using (var stream = File.CreateText(pidFile))
                {
                    stream.Write(Process.GetCurrentProcess().Id);
                };
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://localhost:9999")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .Build();

                host.Run();
                File.Delete(pidFile);
            }
        }
    }
}