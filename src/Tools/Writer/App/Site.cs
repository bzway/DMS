using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using DotLiquid;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Bzway.Writer.App
{
    public class Site
    {
        private readonly List<NavigationNode> navigation;
        private readonly string navigationString;
        private readonly string root;
        private readonly IConfigurationSection config;
        private readonly string source_dir;
        private readonly string public_dir;
        private readonly string layout_dir;
        private readonly string default_layout;
        private readonly string default_category;
        private readonly string date_format;
        private readonly string time_format;
        private readonly Dictionary<string, Template> layouts;
        private readonly List<Page> pages;
        public Site(string root)
        {
            this.root = root;
            this.config = new ConfigurationBuilder()
                .SetBasePath(this.root)
                .AddJsonFile("config.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("docSetting");

            this.default_layout = config.GetValue<string>("default_layout", "default");
            this.default_category = config.GetValue<string>("default_category", "home");
            this.date_format = config.GetValue<string>("date_format", "yyyy-MM-dd");
            this.time_format = config.GetValue<string>("time_format", "hh:mm:ss");

            this.layout_dir = Path.Combine(root, config.GetValue<string>("layout_dir", ".layouts"));
            this.public_dir = Path.Combine(root, config.GetValue<string>("public_dir", ".public"));
            this.source_dir = Path.Combine(root, config.GetValue<string>("source_dir", ""));

            this.layouts = loadLayouts();
            this.pages = this.loadPages();
            this.navigation = new List<NavigationNode>();
            this.navigation.Add(new NavigationNode() { Parent = "", Name = "Root", Path = "#", Sort = 0 });
            var d = Directory.GetDirectories(this.source_dir, "*", SearchOption.AllDirectories);
            foreach (var item in d)
            {
                if (item.StartsWith(this.layout_dir) || item.StartsWith(this.public_dir))
                {
                    continue;
                }
                if (d.Where(m => m.StartsWith(item)).Count() == 1)
                {
                    var path = item.Remove(0, this.source_dir.Length + 1).Split('\\', '/');
                    int i = 0;
                    foreach (var v in path)
                    {
                        if (i == 0)
                        {
                            this.navigation.Add(new NavigationNode() { Parent = "Root", Name = v, Path = "#", Sort = 0 });
                        }
                        else
                        {
                            this.navigation.Add(new NavigationNode() { Parent = path[i - 1], Name = v, Path = "#", Sort = 0 });
                        }
                        i++;
                    }
                }
            }
            foreach (var item in this.pages)
            {
                this.navigation.Add(item.Category);
            }
            StringBuilder st = new StringBuilder();
            st.AppendLine("<ul class='tree'>");
            generateNavigation(this.navigation, string.Empty, st);
            st.AppendLine("</ul>");
            this.navigationString = st.ToString();

            Template.FileSystem = new LayoutFileSystem(new PhysicalFileProvider(this.layout_dir), new PhysicalFileProvider(this.source_dir));
        }


        List<Page> loadPages()
        {
            List<Page> list = new List<Page>();
            try
            {
                Hash hash = new Hash();
                foreach (var item in this.config.GetChildren())
                {
                    hash.Add(item.Key, item.Value);
                }
                foreach (var file in Directory.GetFiles(this.source_dir, "*.*", SearchOption.AllDirectories))
                {
                    if (file.StartsWith(this.layout_dir) || file.StartsWith(this.public_dir) || file.EndsWith("config.json"))
                    {
                        continue;
                    }
                    var page = new Page(this.source_dir, file, hash);
                    list.Add(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        Dictionary<string, Template> loadLayouts()
        {
            Dictionary<string, Template> dict = new Dictionary<string, Template>();
            try
            {
                foreach (var file in Directory.GetFiles(this.layout_dir, "*.html"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (var stream = fileInfo.OpenText())
                    {
                        var template = Template.Parse(stream.ReadToEnd());
                        dict.Add(fileInfo.Name.Remove(fileInfo.Name.Length - 5, 5), template);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dict;
        }

        public void Create(string name)
        {
            var path = Path.Combine(this.source_dir, name + ".md");
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            if (!fi.Exists)
            {
                fi.Create();
            }
            Console.WriteLine("created new page: " + fi.FullName);
        }
        public void Clean()
        {
            Directory.Delete(this.public_dir, true);
        }
        public void Generate()
        {
            foreach (var item in this.pages)
            {
                var template = Template.Parse(item.Source);

                item.LocalSetting.Add("navigation", this.navigationString);
                var temp = template.Render(item.LocalSetting);

                string layout = string.Empty;

                var pageSetting = (Hash)item.LocalSetting["page"];
                if (pageSetting.ContainsKey("layout"))
                {
                    layout = pageSetting["layout"].ToString();
                }
                else
                {
                    var siteSetting = (Hash)item.LocalSetting["site"];

                    if (siteSetting.ContainsKey("default_layout"))
                    {
                        layout = siteSetting["default_layout"].ToString();
                    }
                }

                if (this.layouts.ContainsKey(layout))
                {
                    template = this.layouts[layout];
                    item.LocalSetting.Add("content", temp);
                    temp = template.Render(item.LocalSetting);
                }

                var file = item.FileInfo.FullName.Remove(item.FileInfo.FullName.Length - item.FileInfo.Extension.Length, item.FileInfo.Extension.Length).Remove(0, this.source_dir.Length + 1) + ".html";
                string path = Path.Combine(this.public_dir, file);
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                using (var stream = fileInfo.Create())
                {
                    var buffer = Encoding.UTF8.GetBytes(temp);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            foreach (var path in Directory.GetFiles(this.layout_dir, "*.*", SearchOption.AllDirectories))
            {
                if (path.EndsWith(".html"))
                {
                    continue;
                }
                var destFileName = Path.Combine(this.public_dir, path.Remove(0, this.layout_dir.Length + 1));
                var fi = new FileInfo(destFileName);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                File.Copy(path, destFileName, true);
            }

            var indexFile = Path.Combine(this.public_dir, "index.html");
            File.WriteAllText(indexFile, this.navigationString);

        }

        void generateNavigation(List<NavigationNode> list, string Name, StringBuilder st)
        {
            foreach (var item in list.Where(m => m.Parent == Name).OrderByDescending(m => m.Sort))
            {
                st.AppendLine(string.Format("<li><a href='{1}'> {0}</a></li>", item.Name, item.Path));
                if (list.Where(m => m.Parent == item.Name).Count() > 0)
                {
                    st.AppendLine("<li><ul>");
                    generateNavigation(list, item.Name, st);
                    st.AppendLine("</ul></li>");
                }
            }
        }
        public string[] ignore(string root, string ignoreFile)
        {
            List<string> regList = new List<string>();
            var ignores = File.ReadLines(ignoreFile).Where(m => !m.StartsWith("#") && !string.IsNullOrEmpty(m.Trim())).Select(m => m.Trim()).ToList();
            var rules = ignores.Where(m => m.StartsWith("*.")).Select(m => m.Remove(0, 2)).ToArray();
            if (rules.Length > 0)
            {
                regList.Add(string.Format(@"^.+\.({0})$", string.Join("|", rules)));
            }
            rules = ignores.Where(m => m.StartsWith("/")).Select(m => m.Remove(0, 1)).ToArray();
            if (rules.Length > 0)
            {
                regList.Add(string.Format(@"^\/({0})\/.+$", string.Join("|", rules)));
            }
            rules = ignores.Where(m => m.EndsWith("/")).Select(m => m.Remove(m.Length - 1)).ToArray();
            if (rules.Length > 0)
            {
                regList.Add(string.Format(@"^.?\/({0})\/.+$", string.Join("|", rules)));
            }
            rules = ignores.Where(m => !m.StartsWith("*.") & !m.StartsWith("/") && !m.EndsWith("/")).ToArray();
            if (rules.Length > 0)
            {
                regList.Add(string.Format(@"^.+({0}).+$", string.Join("|", rules)));
            }

            List<string> list = new List<string>();

            var files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories).Where(file =>
            {
                var name = file.Remove(0, root.Length).Replace("\\", "/");
                var result = true;
                foreach (var reg in regList)
                {
                    result = Regex.IsMatch(name, reg);
                    if (result)
                    {
                        return false;
                    }
                }
                return true;
            }).ToArray();
            return files;
        }
    }
}