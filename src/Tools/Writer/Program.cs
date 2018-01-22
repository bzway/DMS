using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using DotLiquid;
using System.Diagnostics;

namespace Bzway.Writer.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appDirectory = AppContext.BaseDirectory;
            var workDirectory = Directory.GetCurrentDirectory();
            Server server = new Server(appDirectory, workDirectory);
            var cmd = args.FirstOrDefault();
            switch (cmd)
            {
                //运行服务
                case null:
                case "--run":
                case "--r":
                case "-run":
                case "-r":
                case "run":
                case "r":
                    server.Run();
                    break;
                //预览文章
                case "--view":
                case "--v":
                case "-view":
                case "-v":
                case "view":
                case "v":
                    var filePath = args.Skip(1).FirstOrDefault();
                    server.Task(0, filePath);
                    break;

                //发布文档
                case "--public":
                case "--p":
                case "-public":
                case "-p":
                case "public":
                case "p":
                //输出文档
                case "--generate":
                case "--g":
                case "-generate":
                case "-g":
                case "generate":
                case "g":
                    var type = args.Skip(1).FirstOrDefault();
                    switch (type)
                    {
                        case "html":
                            break;
                        default:
                            Site site = new Site(workDirectory);
                            site.Generate();
                            Console.WriteLine("generate site OK");
                            break;
                    }
                    break;
                case "clean":
                    new Site(workDirectory).Clean();
                    Console.WriteLine("clean site OK");
                    break;
                default:
                    //创建或编辑一篇文章
                    var docPath = workDirectory;
                    if (cmd.StartsWith("/") || cmd.StartsWith("\\"))
                    {
                        docPath += cmd;
                    }
                    else
                    {
                        docPath += "/" + cmd;
                    }
                    Console.WriteLine(docPath);
                    if (!File.Exists(docPath))
                    {
                        File.WriteAllText(docPath, "hello word");
                    }
                    server.Task(0, cmd);
                    break;
            }

        //<summary>
        //使用XmlSerializer序列化对象
        //</summary>
        //<typeparam name=“T“>需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        //<param name=“obj“>需要序列化的对象</param>
        public static string XmlSerialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 使用XmlSerializer反序列化对象
        /// </summary>
        /// <param name=“xmlOfObject“>需要反序列化的xml字符串</param>
        public static T XmlDeserialize<T>(string xmlOfObject) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                {
                    sr.Write(xmlOfObject);
                    sr.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(ms) as T;
                }
            }
        }
        static void Main(string[] args)
        {
            ETL.Instance.Do(@"D:\Work\DMS\src\tests\ETL\test.json");
            var root = Directory.GetCurrentDirectory();
            var fileProvider = new PhysicalFileProvider(root);
            var tocXml = fileProvider.GetFileInfo("/toc.xml");
            NCX ncx = new NCX()
            {
                head = new List<metaData>(),
                docTitle = new docText() { text = "test" },
                navMap = new List<navPoint>(),
            };
            File.WriteAllText(tocXml.PhysicalPath, XmlSerialize<NCX>(ncx));

            return;
            var toc = fileProvider.GetFileInfo("/toc.ncx");
            if (!toc.Exists)
            {
                File.Create(toc.PhysicalPath).Dispose();
            }

            using (var stream = toc.CreateReadStream())
            {

                // XmlDocument doc = new XmlDocument();
                // XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
                // {
                //     DtdProcessing = DtdProcessing.Ignore
                // };
                // using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                // {
                //     doc.Load(xmlReader);
                // }
                // Xml.Net.XmlConvert

                //var metas = doc.DocumentElement.GetElementsByTagName("meta");
                // var docTitle = doc.GetElementsByTagName("docTitle")[0];
                // var navPoint = doc.GetElementsByTagName("navPoint");
            }


            foreach (var item in fileProvider.GetDirectoryContents("/"))
            {
                if (item.IsDirectory)
                {

                }
            };
            Console.WriteLine(root);
            Console.WriteLine(string.Join("\r\n", args));
            Console.Read();
        }
    }
    [XmlRoot("ncx")]
    public class NCX
    {
        [XmlElement("meta")]
        public List<metaData> head { get; set; }
        public docText docTitle { get; set; }
        public List<navPoint> navMap { get; set; }
    }
    public class metaData
    {
        public string name { get; set; }
        public string content { get; set; }
    }
    public class docText
    {
        public string text { get; set; }
    }
    public class navPoint
    {
        public docText navLabel { get; set; }
        public string Id { get; set; }
        public string Order { get; set; }

        public List<navPoint> navPoints { get; set; }

    }
}