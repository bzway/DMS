using Autofac;
using Bzway.Data.Core;
using Bzway.Common.Share;

namespace Bzway.Data.Mongo
{

    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MongoDatabase>().As<IDatabase>().Named<IDatabase>("MongoDB");
        }
    }
}
