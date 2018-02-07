using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bzway.Common.Share
{
    public class ResourceRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultResourceManager>().As<IResourceManager>();
        }
    }
}
