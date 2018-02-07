using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bzway.Common.Share
{
    public interface IDependencyRegister
    {
        int Order { get; }

        void Register(ContainerBuilder builder);
    } 
}
