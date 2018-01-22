//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Concurrent;
//using Microsoft.Extensions.DependencyInjection;
//using System.Collections.Generic;
//using Bzway.Framework.Application.Entity;
//using Autofac;
//using Autofac.Extensions.DependencyInjection;

//namespace Bzway.Framework.Application
//{

//    public class UserSiteMiddleware
//    {
//        private static Dictionary<string, IServiceProvider> serviceProviderCache = new Dictionary<string, IServiceProvider>();
//        private static object lockObject = new object();
//        private readonly RequestDelegate next;
//        private readonly IServiceCollection gloabServices;

//        public UserSiteMiddleware(RequestDelegate next, IServiceCollection gloabServices)
//        {
//            this.next = next;
//            this.gloabServices = gloabServices;
//        }
//        public Task Invoke(HttpContext context)
//        {
//            //find tenant by host name
//            var siteService = context.RequestServices.GetService<ISiteService>();
//            if (siteService == null)
//            {
//                throw new NotSupportedException("App Service is not registered");
//            }
//            var site = siteService.FindSiteByDomain(context.Request.Host.Value);

//            //try to get ServiceProvider for this site
//            context.RequestServices = TryGetTenantServiceProvider(siteService, site);
//            return next(context);
//        }


//        IServiceProvider TryGetTenantServiceProvider(ISiteService service, Site site)
//        {
//            if (serviceProviderCache.ContainsKey(site.Name))
//            {
//                return serviceProviderCache[site.Name];
//            }
//            lock (lockObject)
//            {
//                if (!serviceProviderCache.ContainsKey(site.Name))
//                {

//                    var containerBuilder = new ContainerBuilder();
//                    containerBuilder.Populate(gloabServices);

//                    ServiceCollection services = new ServiceCollection();
                   
//                    containerBuilder.Populate(services);
//                    containerBuilder.RegisterAssemblyModules();
//                    var container = containerBuilder.Build();
//                    serviceProviderCache.Add(site.Name, container.Resolve<IServiceProvider>());
//                }
//                return serviceProviderCache[site.Name];
//            }
//        }
//    }
//}