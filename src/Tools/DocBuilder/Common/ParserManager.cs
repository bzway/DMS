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
    public static class ParserManager
    {
        static Dictionary<string, Func<string, INewsParser>> NewsParserList = new Dictionary<string, Func<string, INewsParser>>();
        static Dictionary<string, Func<ILinkParser>> LinkParserList = new Dictionary<string, Func<ILinkParser>>();
        static ParserManager()
        {
            NewsParserList.Add("www.ftchinese.com", m => { return new FTNewsParser(m); });
            NewsParserList.Add("mp.weixin.qq.com", m => { return new WechatNewsParser(m); });
            LinkParserList.Add("www.ftchinese.com", () => new FTLinkParser());
        }
        public static INewsParser GetNewsParser(string url)
        {
            Uri uri = new Uri(url);
            if (NewsParserList.ContainsKey(uri.Host))
            {
                return NewsParserList[uri.Host](url);
            }
            return null;
        }

        public static List<string> GetLinks(string url)
        {
            Uri uri = new Uri(url);
            if (LinkParserList.ContainsKey(uri.Host))
            {
                return LinkParserList[uri.Host]().Links;
            }
            else
            {
                return new List<string>() { url };
            }
        }
    }
}