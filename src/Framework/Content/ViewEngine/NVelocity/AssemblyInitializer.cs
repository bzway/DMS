#region License
// 

// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bzway.Core.AppEngine;
using Bzway.Website.Sites.TemplateEngines.NVelocity.MvcViewEngine;
using System.Web.Mvc;
using Bzway.Website.Sites.TemplateEngines.NVelocity;

[assembly: System.Web.PreApplicationStartMethod(typeof(AssemblyInitializer), "Initialize")]
namespace Bzway.Website.Sites.TemplateEngines.NVelocity
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            //ApplicationInitialization.RegisterInitializerMethod(delegate()
            //{
            //    ViewEngines.Engines.Add(NVelocityViewEngine.Default);
            //    Bzway.Website.Sites.View.TemplateEngines.RegisterEngine(new NVelocityTemplateEngine());
            //}, 0);
        }
    }
}
