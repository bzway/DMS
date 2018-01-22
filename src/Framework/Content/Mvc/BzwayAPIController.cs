using System.Web.Http;
using System.Web.Caching;
using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Web;


namespace OpenData.Web.Mvc
{
    public class BzwayAPIController : ApiController
    {
        #region provide
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
    }


    public class BzwayOpenAPIController : ApiController
    {
        #region provide
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        protected internal JsonResult Json(object data)
        {
            return this.Json(data, null, null, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult Json(object data, string contentType)
        {
            return this.Json(data, contentType, null, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return this.Json(data, null, null, behavior);
        }

        protected internal JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return this.Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected internal JsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
        {
            return this.Json(data, contentType, null, behavior);
        }

        protected internal JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }

        protected internal ViewResult View()
        {
            return null;
        }

        protected internal ContentResult Content(string content)
        {
            return this.Content(content, null);
        }

        protected internal ContentResult Content(string content, string contentType)
        {
            return this.Content(content, contentType, null);
        }


        protected internal virtual ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            return new ContentResult { Content = content, ContentType = contentType, ContentEncoding = contentEncoding };
        }

    }
}