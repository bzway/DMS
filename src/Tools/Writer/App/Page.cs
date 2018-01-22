using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using DotLiquid;
using HeyRed.MarkdownSharp;
using Microsoft.Extensions.FileProviders;

namespace Bzway.Writer.App
{
    public class Page
    {
        public string Source { get; set; }
        public Hash LocalSetting { get; private set; }
        public string Root { get; private set; }
        public DateTime CreateTime
        {
            get
            {
                DateTime dt;
                if (this.LocalSetting.ContainsKey("CreateTime"))
                {
                    if (DateTime.TryParse(this.LocalSetting["createtime"].ToString(), out dt))
                    {
                        return dt;
                    }
                }
                return this.FileInfo.CreationTime;
            }
        }
        public NavigationNode Category
        {
            get
            {
                int sort = 0;
                var localSetting = (Hash)this.LocalSetting["page"];
                if (localSetting.ContainsKey("sort"))
                {
                    if (!int.TryParse(localSetting["sort"].ToString(), out sort))
                    {
                        sort = 0;
                    }
                }
                string[] item;
                if (localSetting.ContainsKey("category"))
                {
                    item = localSetting["category"].ToString().Split('\\', '/').Select(m => m.Trim()).ToArray();
                }
                else
                {
                    item = this.FileInfo.FullName.Remove(0, this.Root.Length + 1).Split('\\', '/').Select(m => m.EndsWith(this.FileInfo.Extension) ? m.Remove(m.Length - this.FileInfo.Extension.Length, this.FileInfo.Extension.Length) : m).ToArray();
                }
                var fileName = this.FileInfo.Name.Remove(this.FileInfo.Name.Length - this.FileInfo.Extension.Length, this.FileInfo.Extension.Length);
                var path = string.Join("/", this.FileInfo.DirectoryName.Remove(0, this.Root.Length).Split('\\', '/')) + "/" + fileName + ".html";
                if (item.Length == 1)
                {
                    return new NavigationNode()
                    {
                        Name = string.IsNullOrEmpty(item[item.Length - 1]) ? fileName : item[item.Length - 1],
                        Sort = sort,
                        Parent = "Root",
                        Path = path,
                    };
                }
                return new NavigationNode()
                {
                    Name = string.IsNullOrEmpty(item[item.Length - 1]) ? fileName : item[item.Length - 1],
                    Sort = sort,
                    Parent = string.IsNullOrEmpty(item[item.Length - 2]) ? "Root" : item[item.Length - 2],
                    Path = path,
                };
            }
        }
        public FileInfo FileInfo { get; private set; }
        public Page(string root, string filePath, Hash globalHash)
        {
            this.Root = root;

            this.FileInfo = new FileInfo(filePath);

            StringBuilder sb = new StringBuilder();
            bool isSetting = false;
            Hash localHash = new Hash();
            foreach (var line in File.ReadLines(filePath))
            {
                if (line.Equals("---"))
                {
                    isSetting = !isSetting;
                    continue;
                }
                if (isSetting)
                {
                    var kv = line.Split(':', '=');
                    if (kv.Length != 2)
                    {
                        isSetting = false;
                        continue;
                    }
                    var key = kv[0].Trim();
                    if (string.IsNullOrEmpty(key) || key.Contains(" "))
                    {
                        continue;
                    }
                    var value = kv[1].Trim();
                    if (localHash.ContainsKey(key))
                    {
                        localHash[key] = value;
                    }
                    else
                    {
                        localHash.Add(key, value);
                    }
                }
                else
                {
                    sb.AppendLine(line);
                }
            }
            this.LocalSetting = new Hash();
            this.LocalSetting.Add("site", globalHash);
            this.LocalSetting.Add("page", localHash);

            if (FileInfo.Extension == ".md" || FileInfo.Extension == ".markdown")
            {
                Markdown md = new Markdown()
                {
                    AutoHyperlink = true,
                    LinkEmails = true,
                    DisableEncodeCodeBlock = false,
                    AsteriskIntraWordEmphasis = true,
                    StrictBoldItalic = true
                };

                this.Source = md.Transform(sb.ToString());
            }
            else
            {
                this.Source = sb.ToString();
            }

        }
    }
}