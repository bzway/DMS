using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.Loader;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.FileSystems;
using HeyRed.MarkdownSharp;
using Microsoft.Extensions.Configuration;

namespace Bzway.Common.Script
{

    public class LiquidViewResult : IViewResult
    {
        private static Dictionary<int, Template> cache = new Dictionary<int, Template>();
        private readonly string[] exetensions = new string[] { ".html", ".htm", ".md", ".markdown", ".textile" };
        private readonly string path;
        private readonly int hashCode;
        private readonly string root;
        public IDictionary<string, object> MapData { get; private set; }

        public LiquidViewResult(IDictionary<string, object> dict, string root, string action = "Index", string controller = "Home", string area = "Views")
        {
            if (dict.ContainsKey("area") && dict["area"] != null)
            {
                area = dict["area"].ToString();
            }
            if (dict.ContainsKey("controller") && dict["controller"] != null)
            {
                controller = dict["controller"].ToString();
            }
            if (dict.ContainsKey("action") && dict["action"] != null)
            {
                action = dict["action"].ToString();
            }

            this.root = Path.Combine(Directory.GetCurrentDirectory(), AppEngine.Default.DataFolder, "sites", root, "app");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            this.path = string.Format("{0}/{1}/{2}", area, controller, action);
            this.MapData = dict;
            this.hashCode = (this.root + this.path).GetHashCode();
        }


        public Task<string> Render()
        {
            return Task.Run<string>(() =>
            {
                if (!cache.ContainsKey(this.hashCode))
                {
                    IFileProvider fileProvider = new PhysicalFileProvider(this.root);
                    foreach (var item in exetensions)
                    {
                        var fileInfo = fileProvider.GetFileInfo(path + item);
                        if (fileInfo.Exists)
                        {
                            int count = (int)fileInfo.Length;
                            byte[] buffer = new byte[count];
                            using (var reader = fileInfo.CreateReadStream())
                            {
                                reader.Read(buffer, 0, count);
                            }
                            var source = Encoding.UTF8.GetString(buffer);
                            switch (item)
                            {
                                case ".md":
                                case ".markdown":
                                    Markdown markdown = new Markdown();
                                    source = markdown.Transform(source);

                                    break;

                                default:

                                    break;
                            }
                            var template = Template.Parse(source);
                            template.Registers.Add("file_system", new TemplateFileSystem(fileProvider));
                            cache.Add(this.hashCode, template);
                            break;
                        }
                    }
                    if (!cache.ContainsKey(this.hashCode))
                    {
                        cache.Add(this.hashCode, Template.Parse(""));
                    }
                }
                return cache[this.hashCode].Render(Hash.FromDictionary(this.MapData));
            });
        }
    }


    public class TemplateFileSystem : IFileSystem
    {
        readonly IFileProvider fileProvider;
        public TemplateFileSystem(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public string ReadTemplateFile(Context context, string templateName)
        {
            var path = context[templateName].ToString();

            var fileInfo = fileProvider.GetFileInfo(path);

            if (fileInfo.Exists)
            {
                int count = (int)fileInfo.Length;
                byte[] buffer = new byte[count];
                using (var reader = fileInfo.CreateReadStream())
                {
                    reader.Read(buffer, 0, count);
                }
                var source = Encoding.UTF8.GetString(buffer);
                return source;
            }
            fileInfo = this.fileProvider.GetFileInfo(context["area"].ToString() + path);
            if (fileInfo.Exists)
            {
                int count = (int)fileInfo.Length;
                byte[] buffer = new byte[count];
                using (var reader = fileInfo.CreateReadStream())
                {
                    reader.Read(buffer, 0, count);
                }
                var source = Encoding.UTF8.GetString(buffer);
                return source;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}