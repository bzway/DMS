using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;



namespace OpenData.Framework.Common
{
    public class ExcelResult : ViewResult
    {
        string fileName;
        public ExcelResult(string fileName)
        {
            this.fileName = fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }
            ViewEngineResult viewEngineResult = null;
            if (this.View == null)
            {
                viewEngineResult = this.FindView(context);
                this.View = viewEngineResult.View;
            }
            TextWriter output = context.HttpContext.Response.Output;

            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;


            context.HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.HttpContext.Response.ContentType = "application/ms-excel";



            ViewContext viewContext = new ViewContext(context, this.View, this.ViewData, this.TempData, output);
            this.View.Render(viewContext, output);

            if (viewEngineResult != null)
            {
                viewEngineResult.ViewEngine.ReleaseView(context, this.View);
            }
        }
        protected override ViewEngineResult FindView(ControllerContext context)
        {
            return base.FindView(context);
        }
    }
}