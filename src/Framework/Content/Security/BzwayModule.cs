//using System;
//using System.Security.Principal;
//using System.Threading;
//using System.Web;


//namespace OpenData.Framework.Common
//{
//    public class BzwayModule : IHttpModule, IDisposable
//    {
//        public void Init(HttpApplication context)
//        {
//            context.AuthenticateRequest += context_AuthenticateRequest;
//            context.EndRequest += context_EndRequest;
//        }

//        void context_EndRequest(object sender, EventArgs e)
//        {
//            HttpApplication httpContext = (HttpApplication)sender;
//            httpContext.Response.Headers["Server"]= "Bzway";
//        }

//        void context_AuthenticateRequest(object sender, EventArgs e)
//        {
//            HttpApplication httpContext = (HttpApplication)sender;
//            string token = string.Empty;
//            //从Cookie或Query中得到Token
//            var tokenCookie = httpContext.Request.Cookies["OpenID"];

//            if (tokenCookie != null)
//            {
//                //优先从Cookie中取Token
//                token = tokenCookie.Value;
//            }
//            if (string.IsNullOrEmpty(token))
//            {
//                //其次从Query中取Token
//                token = httpContext.Request.QueryString["OpenID"];
//            }
//            if (string.IsNullOrEmpty(token))
//            {
//                //尝试从Post中取Token
//                token = httpContext.Request.Form["OpenID"];
//            }
//            if (string.IsNullOrEmpty(token))
//            {
//                return;
//            }

//            IPrincipal principal = new BzwayPrincipal(token);
//            Thread.CurrentPrincipal = principal;
//            if (HttpContext.Current != null)
//            {
//                HttpContext.Current.User = principal;
//            }
//        }

//        public void Dispose()
//        {
//        }
//    }

//}