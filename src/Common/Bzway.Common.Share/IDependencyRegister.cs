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
    public class DefaultRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
        }
    }
}
