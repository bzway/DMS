

//using HtmlAgilityPack;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;

//namespace DocBuilder
//{
//    public static class HtmlHelper
//    {
//        public static string GetTitle(this HtmlDocument doc)
//        {
//            var titleNode = doc.DocumentNode.SelectNodes("/html/head/title").FirstOrDefault();
//            if (titleNode == null)
//            {
//                return "";
//            }
//            return titleNode.InnerText;
//        }
//        public static string GetDescription(this HtmlDocument doc)
//        {
//            var descriptionNode = doc.DocumentNode.SelectNodes("/html/head/meta").Where(m => m.Attributes["name"].Value == "description").FirstOrDefault();
//            if (descriptionNode == null)
//            {
//                return "";
//            }
//            return descriptionNode.Attributes["content"].Value;
//        }
//        public static List<string> GetAllLinks(this HtmlDocument doc, string path)
//        {
//            var list = new List<string>();
//            foreach (var item in doc.DocumentNode.SelectNodes("//a[contains(@href)]"))
//            {
//                if (item.Attributes["href"].Value.StartsWith(path))
//                {
//                    list.Add(item.Attributes["href"].Value);
//                }
//            }

//            return list;
//        }
//        public static void GetUrl(string url)
//        {
//            new UrlBuilder(url, 1).Run();
//            foreach (var doc in UrlBuilder.list.Values)
//            {

//                var title = doc.GetTitle();
//                var description = doc.GetDescription();
//                string fileName = Path.Combine(Environment.CurrentDirectory, "Pages", url.GetHashCode().ToString() + ".htm");


//                foreach (var item in doc.DocumentNode.SelectNodes("//a[contains(@href)]"))
//                {
//                    if (item == null)
//                    {
//                        continue;
//                    }
//                    if (item.Name == "script")
//                    {
//                        item.Remove();
//                    }
//                    if (item.Name == "")
//                    { }
//                }

//                doc.Save(fileName);
//            }
//        }
//        public static string DownLoadSource(string url, string type)
//        {
//            string fileName = Path.Combine(Environment.CurrentDirectory, "Content", type, url.GetHashCode().ToString());
//            try
//            {
//                WebClient client = new WebClient();
//                //client.Proxy =  WebProxyHelper.CreateWebProxy();
//                client.DownloadFile(url, fileName);
//            }
//            catch (Exception ex)
//            {
//            }
//            return fileName;
//        }
//    }

//    public class UrlBuilder
//    {
//        public static Dictionary<string, HtmlDocument> list = new Dictionary<string, HtmlDocument>();

//        HtmlWeb web = new HtmlWeb();
//        public Uri OrgianlPage { get; set; }

//        public int Deep { get; set; }

//        public UrlBuilder(string url, int deep = 0)
//        {
//            this.OrgianlPage = new Uri(url);
//            this.Deep = deep;
//        }

//        public void Run()
//        {
//            if (!list.ContainsKey(this.OrgianlPage.ToString()))
//            {
//                list.Add(this.OrgianlPage.ToString(), web.Load(this.OrgianlPage.ToString()));
//            }
//            if (this.Deep > 0)
//            {
//                foreach (var item in list[this.OrgianlPage.ToString()].GetAllLinks(this.OrgianlPage.GetLeftPart(UriPartial.Path)))
//                {
//                    new UrlBuilder(item, this.Deep - 1).Run();
//                }
//            }
//        }
//    }
//}