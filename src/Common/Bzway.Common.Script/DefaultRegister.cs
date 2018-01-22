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
using Bzway.Common.Share;
using Autofac;

namespace Bzway.Common.Script
{
    public partial class DynamicDataRegister : IDependencyRegister
    {
        public const string DynamicDataProvider = "DynamicDataProvider";
        public int Order => 1;
        public void Register(ContainerBuilder builder)
        {
            ScriptEngine e = new ScriptEngine();
            e.AddFunction<string, string, object>("data", (repository, query) => { return string.Format("{0}/{1}/{2}/{3}", repository, query, 0, 10); });
            e.AddFunction<string, string, int, object>("data", (repository, query, index) => { return string.Format("{0}/{1}/{2}/{3}", repository, query, index, 10); });
            e.AddFunction<string, string, int, int, object>("data", (repository, query, index, pageSize) => { return string.Format("{0}/{1}/{2}/{3}", repository, query, index, pageSize); });
            builder.RegisterInstance<ScriptEngine>(e).SingleInstance().Named<ScriptEngine>(DynamicDataRegister.DynamicDataProvider);
        }
    }
}