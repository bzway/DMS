using Autofac;
using Bzway.Data.Core;
using Bzway.Common.Share;

namespace Bzway.Data.SQLServer
{
    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<SQLServerDatabase>().As<IDatabase>().Named<IDatabase>("SQLServer");
        }
    }
}