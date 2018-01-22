using Autofac;
using Bzway.Common.Share;
using Bzway.Database.Core;

namespace Bzway.Database.Mongo
{
    public class MongoDatabaseRegister : IDependencyRegister
    {
        public const string ProviderName = "Mongo";
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MongoDatabaseProvider>().As<IDatabaseProivder>().Named<IDatabaseProivder>(ProviderName);
        }
    }
}
