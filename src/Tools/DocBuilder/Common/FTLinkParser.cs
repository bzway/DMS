using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Common.Share
{

    public class FTLinkParser : ILinkParser
    {
        static readonly HtmlWeb webclient = new HtmlWeb();
        static readonly Dictionary<int, string> dict = new Dictionary<int, string>();

        private readonly string url;
        private readonly HtmlDocument htmlDocument;

        public FTLinkParser()
        {
            this.url = "http://www.ftchinese.com/";
            this.htmlDocument = webclient.Load(this.url);

            foreach (var item in htmlDocument.DocumentNode.SelectNodes("//a"))
            {
                var href = item.GetAttributeValue("href", "");
                if (href.Contains("/story/"))
                {
                    href = this.url + href;
                    var key = href.GetHashCode();
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, href);
                    }
                }
            }

        }

        public List<string> Links
        {
            get
            {
                return dict.Values.ToList();
            }
        }
    }

}