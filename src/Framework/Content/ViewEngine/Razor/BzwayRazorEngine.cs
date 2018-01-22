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
using System.Web.Mvc;

namespace OpenData.Framework.Common.ViewEngine.Razor
{
    public class BzwayRazorEngine : IBzwayViewEngine
    {
        public override string ToString()
        {
            return "Razor";
        }
        public string GetFileExtensionForLayout()
        {
            return ".cshtml";
        }

        public string GetFileExtensionForView()
        {
            return ".cshtml";
        }

        public IEnumerable<string> FileExtensions
        {
            get { return new string[] { ".cshtml" }; }
        }

        public IViewEngine GetViewEngine()
        {
            return new RazorViewEngine();
        }

        public IView GetView(ControllerContext controllerContext, string viewVirtualPath, string masterVirtualPath)
        {
            return new RazorView(controllerContext, viewVirtualPath, masterVirtualPath, false, null);
        }
    }
}