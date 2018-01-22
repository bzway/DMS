using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace OpenData.Framework.Common
{
    /// <summary>
    /// 生成静态文件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SiteAttribute : ActionFilterAttribute
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 方法
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

                
            StreamWriter streamWriter = null;
            try
            {
                ViewResult viewResult = filterContext.Result as ViewResult;
                string file = filterContext.HttpContext.Server.MapPath(string.Format("~/html/{0}.html", filterContext.HttpContext.Request.Url.GetHashCode()));
                streamWriter = new StreamWriter(file, false, Encoding.UTF8);
                streamWriter.WriteLine(string.Format("<!-- {0} -->", DateTime.Now));
                var viewContext = new ViewContext(filterContext.Controller.ControllerContext, viewResult.View, viewResult.ViewData, viewResult.TempData, streamWriter);
                viewResult.View.Render(viewContext, streamWriter);
            }
            catch (Exception ex)
            {
                log.Error("GenerateHTMLAttribute", ex);
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }




        }
        #endregion

 
    }
}