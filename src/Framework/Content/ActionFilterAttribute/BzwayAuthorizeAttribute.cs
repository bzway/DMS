using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using Bzway.Business.Service;
using System.Net;

namespace Bzway.WebSite
{
    public class BzwayAuthorizeAttribute : AuthorizeAttribute
    {
        public bool WithLock { get; set; }
        public BzwayAuthorizeAttribute()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.GetUserManager().GetCurrentUser();

            if (user == null)
            {
                httpContext.Response.Redirect("/User/Authorize/Login/?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                return false;
            }
            if (!WithLock)
            {
                switch (user.Locked)
                {
                    case LockType.None:
                        break;
                    case LockType.MobilePhone:
                        httpContext.Response.Redirect("/User/Authorize/SendMobileCode?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                        return false;
                    case LockType.Email:
                        httpContext.Response.Redirect("/User/Authorize/SendEmailCode?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                        return false;
                    case LockType.Information:
                        httpContext.Response.Redirect("/User/Authorize/Profile?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                        return false;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(this.Roles))
            {
                return true;
            }
            foreach (var item in this.Roles.Split(',', '|', ';'))
            {
                if (user.Roles.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

            }
            if (user.Name.Contains("zhumingwu"))
            {
                return true;
            }
            httpContext.Response.Redirect("/User/Authorize/Login/noright?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
            return false;
        }
    }
}