using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using Autofac;

namespace System
{
    public class AppEngine
    {
        public static Autofac.IContainer Current;
        public static T GetService<T>(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return Current.Resolve<T>();
            }
            return Current.ResolveNamed<T>(name);
        }
    }
}
