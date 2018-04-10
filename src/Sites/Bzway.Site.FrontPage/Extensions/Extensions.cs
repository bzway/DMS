using Bzway.Common.Script;
using Bzway.Framework.Application.Entity;
using System.Collections.Generic;
using Bzway.Data.Core;

namespace Bzway.Sites.FrontPage.Controllers
{
    public static class Extensions
    {
        static Dictionary<string, UrlMaper> dict = new Dictionary<string, UrlMaper>();
        public static UrlMaper GetMaper(this Site site)
        {
            if (!dict.ContainsKey(site.Name))
            {
                if (site.Name == "ebook")
                {
                    var build = new UrlMapBuilder()
                        .Map("/$Area=Defalut/$Controller?=Home/$Action?=Index")
                        .Build();
                    dict[site.Name] = build;
                }
                else
                {
                    var build = new UrlMapBuilder()
                        //.Map("/$Action")
                        //.Map("/$Controller")
                        .Map("/$Controller/$Action/")
                        .Map("/$Controller/$Action/$Id:int", new KeyValuePair<string, string>("data", "product"))
                        .Build();
                    dict[site.Name] = build;
                }
            }
            return dict[site.Name];
        }
    }
}