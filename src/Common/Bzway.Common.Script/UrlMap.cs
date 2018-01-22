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
    public class UrlMap
    {
        public string UrlTemplate { get; private set; }
        public Dictionary<string, string> Defaults { get; private set; }
        public List<Segment> Segments { get; private set; }
        public UrlMap(string template, params KeyValuePair<string, string>[] defaults)
        {
            this.Defaults = new Dictionary<string, string>();
            foreach (var item in defaults)
            {
                this.Defaults.Add(item.Key, item.Value);
            }
            this.UrlTemplate = template;
            this.Segments = GetSegments();
        }
        List<Segment> GetSegments()
        {
            List<Segment> list = new List<Segment>();
            foreach (var item in this.UrlTemplate.Split('/'))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                list.Add(new Segment(item));
            }
            return list;
        }
    }
}