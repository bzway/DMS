using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bzway.Common.Share
{
    public class WechatNewsParser : INewsParser
    {
        private readonly static HtmlWeb webclient = new HtmlWeb();
        private readonly string url;
        private readonly HtmlDocument doc;
        public WechatNewsParser(string url)
        {
            this.url = url;
            this.doc = webclient.Load(url);
        }


        public string Author
        {
            get
            {
                var node = doc.GetElementbyId("post-user");
                if (node == null)
                {
                    return string.Empty;
                }
                return node.InnerText;
            }
        }

        public string Content
        {
            get
            {
                var titleNode = doc.GetElementbyId("js_content");
                if (titleNode == null)
                {
                    return string.Empty;
                }
                return titleNode.InnerHtml;
            }
        }

        public string Digest
        {
            get
            {
                return string.Empty;
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
                var node = doc.DocumentNode.SelectNodes("//*[@id=\"js_content\"]/*/img").FirstOrDefault();
                if (node == null)
                {
                    return "";
                }
                return node.GetAttributeValue("data-src", "");
            }
        }

        public string Title
        {
            get
            {

                var node = doc.DocumentNode.SelectNodes("/html/head/title").FirstOrDefault();
                if (node == null)
                {
                    return string.Empty;
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