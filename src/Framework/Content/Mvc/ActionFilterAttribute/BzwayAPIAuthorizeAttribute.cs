
//using System;
//using System.Web;
//using System.Web.Http;
//using System.Web.Mvc;
//namespace Bzway.Web.Mvc
//{
//    public class BzwayAPIAuthorizeAttribute : AuthorizeAttribute
//    {

//        public BzwayAPIAuthorizeAttribute()
//        {
//        }

//        protected override bool IsAuthorized(HttpActionContext actionContext)
//        {
//            var requestToken = BzwayUser.CurrentUserToken;
//            if (!requestToken.IsActive)
//            {
//                //actionContext.Response.StatusCode = (BzwayConfiguration.Settings.LoginUrl);
//                return false;
//            }
//            if (requestToken.Roles.HasFlag(role))
//            {
//                return true;
//            }
//            else
//            {
//                //actionContext.Response.Redirect(BzwayConfiguration.Settings.LoginUrl);
//                return false;
//            }
//            return true;
//        }
//    }
//}