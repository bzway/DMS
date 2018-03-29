using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bzway.Module.Wechat.Interface;
using Microsoft.Extensions.Logging;
using System.Threading;
using Bzway.Framework.Application;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bzway.Common.Share.Collections;
using System.Diagnostics;

namespace Bzway.Sites.BackOffice.Controllers
{

    public class HomeController : BaseController<HomeController>
    {
        #region ctor
        readonly ITenant tenant;
        readonly IAuthenticationService authentication;
        public HomeController(
            ITenant tenant,
            ISiteService siteService,
            IAuthenticationService authentication,
            ILoggerFactory loggerFactory) : base(loggerFactory, siteService)
        {
            this.authentication = authentication;
            this.tenant = tenant;
        }
        #endregion

        static IQuickSearch<MobileModel> search;

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string Mobile)
        {
            List<MobileModel> list = new List<MobileModel>();
            if (search == null)
            {
                foreach (var item in System.IO.File.ReadAllLines("d:\\mobile.txt"))
                {
                    //1,1300000,山东,济南,中国联通,0531,250000
                    var data = item.Split(',');
                    list.Add(new MobileModel()
                    {
                        Prefix = int.Parse(data[1]),
                        Proince = data[2],
                        City = data[3],
                        Vendor = data[4],
                        AreaCode = data[5],
                        PostCode = data[6],
                    });
                }
                search = QuickSearch.BuildQuickSearch<MobileModel>(list.ToArray());
            }
            list = new List<MobileModel>();
            foreach (var item in Mobile.Split(','))
            {
                var x = search.Search(new MobileModel() { Prefix = int.Parse(item.Substring(0, 7)) });
                if (x != null)
                {
                    list.Add(x);
                }
            }
            //return;
            return View(list);
        }



        public class MobileModel : IComparable<MobileModel>
        {
            public string Proince { get; set; }
            public string City { get; set; }
            public string Vendor { get; set; }
            public string PostCode { get; set; }
            public string AreaCode { get; set; }

            public int Prefix { get; set; }

            public int CompareTo(MobileModel other)
            {
                return this.Prefix.CompareTo(other.Prefix);
            }

            public override int GetHashCode()
            {
                return Prefix;
            }
        }
        public async Task<IActionResult> Login()
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();

            var identity = new ClaimsIdentity("OpenApiAuthentication");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "OpenApiAuthentication"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "OpenApiAuthentication"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "test"));
            principal.AddIdentity(identity);
            await this.authentication.SignInAsync(this.HttpContext,
                "OpenApiAuthentication",
                principal, new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    RedirectUri = "/"
                });

            return Content("Ok");
        }

    }

    public class AccountController : BaseController<HomeController>
    {
        #region ctor
        readonly ITenant tenant;
        public AccountController(
            ITenant tenant,
            ISiteService siteService,
            ILoggerFactory loggerFactory) : base(loggerFactory, siteService)
        {
            this.tenant = tenant;
        }
        #endregion

        public async Task<IActionResult> Login(string code, string state, string url)
        {
            //如果没有请求码
            if (string.IsNullOrEmpty(code))
            {
                //get request code
                var authorizedUrl = this.Request.Path + "/WechatBind/Authorized?url=" + "todo";
                var redirecturl = "";
                return this.Redirect(redirecturl);
            }
            //如果状态码有误
            if (!string.Equals(state, ""))
            {
                return Redirect("/Error/Index");
            }
            //request access token
            var token = "test";//todo get access token from auth server according request code.
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Error/Index");
            }
            //request user profile
            var roles = "Admin,TEST";//todo get user profile according to access token.
            ClaimsPrincipal principal = new ClaimsPrincipal();
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token));
            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, "test"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "test"));
            foreach (var item in roles.Split(','))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }

            principal.AddIdentity(identity);
            return this.SignIn(principal, new AuthenticationProperties() { }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
