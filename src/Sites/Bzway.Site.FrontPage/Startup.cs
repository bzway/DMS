﻿using Bzway.Common.Share;
using Bzway.Framework.Application;
using Bzway.Framework.Content;
using Bzway.Module.Wechat.Interface;
using Bzway.Module.Wechat.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bzway.Sites.FrontPage
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            //Add framework services.
            services.AddMultiTenant();
            services.AddMvc();
            return services.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                //Front Page Dev
                routes.MapRoute(
                    name: "Developement",
                    template: "dev/{*PageUrl}",
                    defaults: new { controller = "Development", action = "Index", PageUrl = "" }
                );
                //Front Page Uat
                routes.MapRoute(
                    name: "Staging",
                    template: "uat/{*PageUrl}",
                    defaults: new { controller = "Staging", action = "Index", PageUrl = "" }
                );
                //Front Page
                routes.MapRoute(
                    name: "Page",
                    template: "{*PageUrl}",
                    defaults: new { controller = "Home", action = "Index", PageUrl = "" }
                );
            });
        }
    }
}
