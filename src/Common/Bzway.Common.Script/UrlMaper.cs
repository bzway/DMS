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
using Microsoft.AspNetCore.Http;

namespace Bzway.Common.Script
{
    public class UrlMaper
    {
        private readonly List<UrlMap> list;
        public UrlMaper(List<UrlMap> list)
        {
            this.list = list;
        }
        public IViewResult Action(string url, string root)
        {
            if (url == null)
            {
                url = string.Empty;
            }
            //当得当前路径的片段
            var segments = url.Split('/').Where(m => !string.IsNullOrEmpty(m)).ToArray();
            foreach (var item in list)
            {
                if (item.Segments.Where(m => !m.AllowNull).Count() != segments.Length)
                {
                    continue;
                }
                int i = 0;
                bool isMatch = true;
                Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (var segment in item.Segments)
                {
                    MatchResult<KeyValuePair<string, object>> matchResult;
                    if (i >= segments.Length)
                    {
                        matchResult = segment.IsMatch(string.Empty);
                    }
                    else
                    {
                        matchResult = segment.IsMatch(segments[i]);
                    }
                    i++;
                    isMatch &= matchResult.Result;
                    if (!isMatch)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(matchResult.Data.Key))
                    {
                        continue;
                    }
                    if (!dict.ContainsKey(matchResult.Data.Key))
                    {
                        dict.Add(matchResult.Data.Key, matchResult.Data.Value);
                    }
                }
                if (isMatch)
                {
                    foreach (var kv in item.Defaults)
                    {
                        if (dict.ContainsKey(kv.Key))
                        {
                            dict[kv.Key] = kv.Value;
                        }
                        else
                        {
                            dict.Add(kv.Key, kv.Value);
                        }
                    }
                    LiquidViewResult result = new LiquidViewResult(dict, root);
                    return result;
                }
            }
            return null;
        }
        public string Url(string vpath)
        {
            return string.Empty;
        }
    }
}