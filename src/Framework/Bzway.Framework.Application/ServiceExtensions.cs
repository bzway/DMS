using Microsoft.AspNetCore.Http;
using Bzway.Framework.Application;
using System.IO;
using Bzway.Common.Share;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using System.Linq;
using System;
using System.Security.Principal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void AddMultiTenant(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal, BzwayPrincipal>();
            services.AddScoped<ITenant, Tenant>();
        }

        public static IServiceProvider Build(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            var folder = Directory.GetCurrentDirectory();

            ITypeFinder finder = new TypeFinder();
            foreach (var register in finder.Find<IDependencyRegister>(Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories)).OrderBy(m => m.Order))
            {
                register.Register(builder);
            };

            AppEngine.Current = builder.Build();
            return new AutofacServiceProvider(AppEngine.Current);
        }

        public static BzwayPrincipal Principal(this HttpContext httpContext)
        {
            return httpContext.User as BzwayPrincipal;
        }
    }
}