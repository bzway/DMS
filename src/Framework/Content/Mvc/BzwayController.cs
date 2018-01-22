using System;
using System.Web.Mvc;
using OpenData.Caching;
using OpenData.AppEngine;
using System.Web;
using System.Threading;


namespace OpenData.Web.Mvc
{
    public class BzwayController : Controller
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :   
                        null;
            }
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string VerificationCode
        {
            get
            {
                var code = this.Session["ValidateCode"];
                if (code == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Session["ValidateCode"].ToString();
                }
            }
            set
            {
                this.Session["ValidateCode"] = value;
            }
        }
        public string MyMessage
        {
            get
            {

                var msg = this.Session["MyMessage"];
                if (msg == null)
                {
                    return string.Empty;
                }
                else
                {
                    this.Session["MyMessage"] = null;
                    return msg.ToString();
                }
            }
            set
            {
                this.Session["MyMessage"] = value;
            }
        }

        public bool NeedVerificationCode(string key)
        {
            key += GetIP();
            var c = ApplicationEngine.Current.Resolve<ICacheManager>();
            var count = c.Get<int>(key);
            count++;
            c.Set(key, count, 1);
            if (count > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetIP()
        {
            if (string.IsNullOrEmpty(this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                return this.Request.UserHostAddress;
            }
            else
            {
                return this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }

        #region 获取路径
        public string GetAppPath()
        {
            string path = GetAppPathWithOutSlash();
            if (path.Substring(path.Length - 1, 1) != "/") path += "/";
            return path;
        }

        public string GetAppPathWithOutSlash()
        {
            string path = "http://" + Request.Url.Host;
            int port = Request.Url.Port;
            if (port != 80) path += ":" + port.ToString();
            path += Request.ApplicationPath;
            return path;
        }
        #endregion
    }

}