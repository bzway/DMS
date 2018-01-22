using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using OpenData.Framework.Common.ViewEngine;
using System.Diagnostics;

namespace OpenData.Framework.Common
{
    public class FrontViewResult : ViewResult
    {
        private IViewEngine viewEngine;
        public FrontViewResult(ControllerContext controllerContext, string fileExtension, string viewVirtualPath)
            : this(controllerContext, fileExtension, viewVirtualPath, "")
        {
        }
        public FrontViewResult(ControllerContext controllerContext, string fileExtension, string viewVirtualPath, string masterVirtualPath)
        {
            var templateEngine = ViewEngineManager.GetEngineByFileExtension(fileExtension);
            this.View = templateEngine.GetView(controllerContext, viewVirtualPath, masterVirtualPath);
            this.viewEngine = templateEngine.GetViewEngine();
        }
        public override void ExecuteResult(ControllerContext context)
        {
#if TRACE
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            context.HttpContext.Response.Write("ExecuteResult Start.</br>");
#endif
            base.ExecuteResult(context);
#if TRACE
            stopwatch.Stop();
            context.HttpContext.Response.Write(string.Format("ExecuteResult, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
        }
        protected override ViewEngineResult FindView(ControllerContext context)
        {
            return new ViewEngineResult(View, viewEngine);
        }
    }
}