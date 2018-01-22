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
using DotLiquid;
using DotLiquid.FileSystems;

namespace Bzway.Common.Script
{

    public class ViewEngine : IViewEngine
    {
        public ViewEngine()
        {
        }

        public IViewResult View()
        {
            return new LiquidViewResult(null, "");
        }
    }

}