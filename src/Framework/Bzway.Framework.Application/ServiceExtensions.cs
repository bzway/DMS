using Microsoft.AspNetCore.Http;
using Bzway.Framework.Application;
using System.IO;
using Bzway.Common.Share;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using System.Linq;
using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void AddMultiTenant(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal, BzwayPrincipal>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
            //.AddScheme<CookieAuthenticationOptions, OpenAuthenticationHandler>(
            //    "OpenApiAuthentication",
            //    options =>
            //    {
            //        options.ClaimsIssuer = "";
            //        options.CookieManager = new ChunkingCookieManager();
            //        options.Cookie.Name = "Cookies";
            //        options.LoginPath = new PathString("/Account/Login");
            //        options.LogoutPath = new PathString("/Account/Logout");
            //        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            //        options.ReturnUrlParameter = "ReturnUrl";
            //        options.TicketDataFormat = new TicketDataFormat(new EphemeralDataProtectionProvider().CreateProtector("test"));

            //    });
            //services.AddScoped<IPrincipal>(m =>
            //{
            //    var authentication = m.GetService<OpenApiAuthenticationHandler>();
            //    var principal = authentication.AuthenticateAsync().Result.Principal;
            //    return principal;
            //});
            services.AddScoped<ITenant, Tenant>();
        }

        public static IServiceProvider Build(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            ITypeFinder finder = new TypeFinder();
            services.AddSingleton(typeof(ITypeFinder), finder);
            builder.Populate(services);
            foreach (var register in finder.Find<IDependencyRegister>().Select(m => (IDependencyRegister)Activator.CreateInstance(m)).OrderBy(m => m.Order))
            {
                register.Register(builder);
            };
            AppEngine.Current = builder.Build();
            return new AutofacServiceProvider(AppEngine.Current);
        }
    }
}