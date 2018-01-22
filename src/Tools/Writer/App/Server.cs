using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using DotLiquid;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace Bzway.Writer.App
{
    public class Server
    {
        private readonly string processFile;
        private readonly string workDirectory;
        private readonly string appDirectory;
        private readonly string HostUrl;
        private readonly string BroswerPath;
        public Server(string appDirectory, string workDirectory)
        {
            this.workDirectory = workDirectory;
            this.appDirectory = appDirectory;
            var config = new ConfigurationBuilder()
                .SetBasePath(this.appDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("appSetting");
            this.HostUrl = config.GetValue<string>("HostUrl", "http://localhost:9999");
            this.BroswerPath = config.GetValue<string>("BroswerPath", @"D:\Program Files\Chrome\chrome.exe");
            this.processFile = this.appDirectory + "/" + config.GetValue<string>("processFile", "writer.pid");
        }

        public void Run()
        {
            bool canRun = !File.Exists(this.processFile);
            if (!canRun)
            {
                int pid = 0;
                if (int.TryParse(File.ReadAllText(this.processFile), out pid))
                {
                    var process = Process.GetProcesses().FirstOrDefault(m => m.Id == pid);
                    canRun = (process == null);
                }
            }
            if (canRun)
            {
                File.WriteAllText(this.processFile, Process.GetCurrentProcess().Id.ToString());
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls(this.HostUrl)
                    .UseContentRoot(this.workDirectory)
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();
                host.Run();
                File.Delete(this.processFile);
            }
        }

        public void Task(int type, string path)
        {

        }
    }
}