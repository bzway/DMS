using System.Diagnostics;
using System.IO;
using System.Text;
namespace AutoCode
{
    public static class Program
    {
        static void Main(string[] arg)
        {
            var workPath = System.Environment.CurrentDirectory;
            var cmdBuilder = new StringBuilder();
            cmdBuilder.AppendLine("@echo off");
            cmdBuilder.AppendLine(@"@Set PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;%PATH:C:\Windows\Microsoft.NET\Framework\v4.0.30319;=%");
            cmdBuilder.AppendLine(@"echo restore packages...");
            cmdBuilder.AppendLine(@"nuget restore .\OpenData.sln");
            cmdBuilder.AppendLine(@"ECHO delete old files");
            cmdBuilder.AppendLine(@"rd /s /q .\Publish");
            cmdBuilder.AppendLine(@"");
            foreach (var projectFile in Directory.GetFiles(workPath, "*.csproj", SearchOption.AllDirectories))
            {
                FileInfo file = new FileInfo(projectFile);
                foreach (var publishFile in file.Directory.GetFiles("*.pubxml", SearchOption.AllDirectories))
                {
                    cmdBuilder.AppendLine(@"MSBuild " + projectFile + " /p:DeployOnBuild=true /p:PublishProfile=" + publishFile);
                }
            }
            var batFile = Path.Combine(workPath, "publish.bat");
            File.WriteAllText(batFile, cmdBuilder.ToString());
            Process.Start(batFile);
        }
    }
}