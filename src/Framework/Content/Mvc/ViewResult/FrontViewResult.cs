using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Bzway.Web.ViewEngine;



namespace OpenData.Mvc
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
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            base.ExecuteResult(context);
#if Page_Trace
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