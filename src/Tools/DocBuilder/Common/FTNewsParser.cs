using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bzway.Common.Share
{
    public class FTNewsParser : INewsParser
    {
        private readonly static HtmlWeb webclient = new HtmlWeb();
        private readonly string url;
        private readonly HtmlDocument htmlDocument;
        public FTNewsParser(string url)
        {
            if (!url.Contains("?full=y"))
            {
                url += "?full=y";
            }
            this.url = url;
            this.htmlDocument = webclient.Load(url);
        }
        public string Author
        {
            get
            {
                return "金融时报";
            }
        }

        public string Content
        {
            get
            {
                var node = htmlDocument.DocumentNode.SelectNodes("/html/body/div[7]/div/div[1]/div/div[1]/div[6]").FirstOrDefault();
                if (node == null)
                {
                    return "";
                }
                return node.InnerText;
            }
        }

        public string Digest
        {
            get
            {
                var node = htmlDocument.DocumentNode.SelectNodes("/html/body/div[7]/div/div[1]/div/div[1]/div[2]").FirstOrDefault();
                if (node == null)
                {
                    return "";
                }
                return node.InnerText;
            }
        }

        public string Id
        {
            get
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(this.url);
                    return BitConverter.ToString(md5.ComputeHash(bytes));
                }
            }
        }

        public string ThumbMedia
        {
            get
            {
                var node = htmlDocument.DocumentNode.SelectNodes("/html/body/div[7]/div/div[1]/div/div[1]/div[3]/figure").FirstOrDefault();
                if (node == null)
                {
                    return "";
                }
                return node.GetAttributeValue("data-url", "");
            }
        }

        public string Title
        {
            get
            {

                var node = htmlDocument.DocumentNode.SelectNodes("/html/body/div[7]/div/div[1]/div/div[1]/h1").FirstOrDefault();
                if (node == null)
                {
                    return "";
                }
                return node.InnerText;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
        }
    }
}