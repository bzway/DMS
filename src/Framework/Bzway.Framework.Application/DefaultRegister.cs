using Autofac;
using Bzway.Common.Share;
using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bzway.Framework.Application
{

    public class DefaultRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<RedisCacheManager>().SingleInstance().As<ICacheManager>().Named<ICacheManager>("Redis");
            builder.RegisterType<DefaultCache>().SingleInstance().As<ICacheManager>();
        }
    }
}
