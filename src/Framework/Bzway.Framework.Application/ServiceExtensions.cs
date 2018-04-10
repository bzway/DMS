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
            services.AddScoped<IPrincipal>(m => m.GetRequiredService<IHttpContextAccessor>().HttpContext.User);
            services.AddAuthentication(m =>
            {
                m.DefaultScheme = OpenAuthenticationOptions.DefaultSchemeName;
                m.AddScheme<OpenAuthenticationHandler>(OpenAuthenticationOptions.DefaultSchemeName, OpenAuthenticationOptions.DefaultSchemeName);
            });

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
            var container = builder.Build();
            AppEngine.Default.Init(container);
            return new AutofacServiceProvider(container);
        }
    }
}