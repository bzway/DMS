using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace System
{
    public class AppEngine
    {

        static readonly Lazy<AppEngine> lazy = new Lazy<AppEngine>(() => { return new AppEngine(); });
        IContainer container;
        static Func<IContainer> ctor;
        AppEngine()
        {

        }

        public static AppEngine Default
        {
            get
            {
                return lazy.Value;
            }
        }
        public IConfigurationRoot Configuration { get; set; }
        private string dataFolder;
        public string DataFolder
        {
            get
            {
                if (string.IsNullOrEmpty(dataFolder))
                {
                    var section = this.Configuration.GetSection("SiteSetting");
                    this.dataFolder = section.GetValue<string>("DataFolder");
                }
                return this.dataFolder;
            }
        }

        public void Init(IContainer container)
        {
            this.container = container;
        }

        public T GetService<T>(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.Resolve<T>();
            }
            return container.ResolveNamed<T>(name);
        }
    }
}
