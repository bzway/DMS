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
    public class UrlMapBuilder : IUrlMapBuilder
    {
        List<UrlMap> list = new List<UrlMap>();
        /// <summary>
        /// Get 'url template' with defaults:[]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="defaults">定义Area, Controller, Action, Data, Where, PageIndex, PageSize</param>
        /// <returns></returns>
        public UrlMapBuilder Map(string template, params KeyValuePair<string, string>[] defaults)
        {
            list.Add(new UrlMap(template,defaults) { });
            return this;
        }
        public UrlMaper Build()
        {
            return new UrlMaper(this.list);
        }
    }
}