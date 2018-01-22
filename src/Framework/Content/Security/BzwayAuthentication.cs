using System;
using System.Web;


namespace OpenData.Framework.Common
{
    public sealed class BzwayAuthentication
    {
        /// <summary>Gets the name of the cookie used to store the forms-authentication ticket.</summary>
        /// <returns>The name of the cookie used to store the forms-authentication ticket. The default is ".ASPXAUTH".</returns>
        public static string CookieName { get; set; }
        /// <summary>Gets the path for the forms-authentication cookie.</summary>
        /// <returns>The path of the cookie where the forms-authentication ticket information is stored. The default is "/".</returns>
        public static string CookiePath { get; set; }
        /// <summary>Gets a value indicating whether the forms-authentication cookie requires SSL in order to be returned to the server.</summary>
        /// <returns>true if SSL is required to return the forms-authentication cookie to the server; otherwise, false. The default is false.</returns>
        public static bool RequireSSL { get; set; }
        /// <summary>Gets the value of the domain of the forms-authentication cookie.</summary>
        /// <returns>The <see cref="P:System.Web.HttpCookie.Domain" /> of the forms-authentication cookie. The default is an empty string ("").</returns>
        public static string CookieDomain { get; set; }
        /// <summary>Gets the URL for the login page that the <see cref="T:System.Web.Security.FormsAuthentication" /> class will redirect to.</summary>
        /// <returns>The URL for the login page that the <see cref="T:System.Web.Security.FormsAuthentication" /> class will redirect to. The default is "login.aspx."</returns>
        public static string LoginUrl { get; set; }
        /// <summary>Gets the URL that the <see cref="T:System.Web.Security.FormsAuthentication" /> class will redirect to if no redirect URL is specified.</summary>
        /// <returns>The URL that the <see cref="T:System.Web.Security.FormsAuthentication" /> class will redirect to if no redirect URL is specified. The default is "default.aspx."</returns>
        public static string DefaultUrl { get; set; }
        /// <summary>Initializes the <see cref="T:System.Web.Security.FormsAuthentication" /> object based on the configuration settings for the application.</summary>

        /// <summary>Removes the forms-authentication ticket from the browser.</summary>
        public static void SignOut()
        {
            var httpContext = HttpContext.Current;
            HttpCookie myCookie = httpContext.Request.Cookies["OpenID"];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                httpContext.Response.Cookies.Add(myCookie);
            }

            //httpContext.Response.Cookies.Clear();
            //httpContext.Request.Cookies.Clear();
            httpContext.Session.Abandon();
            //httpContext.Response.Redirect(BzwayConfiguration.Settings.LoginUrl, false);
            //httpContext.Response.End();
        }
        /// <summary>Creates an authentication ticket for the supplied user name and adds it to the cookies collection of the response, or to the URL if you are using cookieless authentication.</summary>
        /// <param name="userName">The name of an authenticated user. This does not have to map to a Windows account. </param>
        /// <param name="createPersistentCookie">true to create a persistent cookie (one that is saved across browser sessions); otherwise, false. </param>
        /// <exception cref="T:System.Web.HttpException">
        ///   <see cref="P:System.Web.Security.FormsAuthentication.RequireSSL" /> is true and <see cref="P:System.Web.HttpRequest.IsSecureConnection" /> is false.</exception>

        /// <summary>Creates an authentication ticket for the supplied user name and adds it to the cookies collection of the response, using the supplied cookie path, or using the URL if you are using cookieless authentication.</summary>
        /// <param name="userName">The name of an authenticated user. </param>
        /// <param name="createPersistentCookie">true to create a durable cookie (one that is saved across browser sessions); otherwise, false. </param>
        /// <param name="cookiePath">The cookie path for the forms-authentication ticket.</param>
        /// <exception cref="T:System.Web.HttpException">
        ///   <see cref="P:System.Web.Security.FormsAuthentication.RequireSSL" /> is true and <see cref="P:System.Web.HttpRequest.IsSecureConnection" /> is false.</exception>
        public static void SetAuthCookie(UserIdentity ticket, bool createPersistentCookie=true, string cookiePath = "/")
        {
            var httpContext = HttpContext.Current;
            HttpCookie authenticationCookie = new HttpCookie("OpenID");
            authenticationCookie.Value = ticket.ToString();
            authenticationCookie.Expires = DateTime.Now.AddYears(10);
            authenticationCookie.Path = cookiePath;
            httpContext.Response.Cookies.Set(authenticationCookie);
        }

    }
}