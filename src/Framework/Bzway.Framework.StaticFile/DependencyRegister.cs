using Autofac;
using Bzway.Common.Share;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public class DependencyRegister : IDependencyRegister
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder)
        {
            MasterConnection master = new MasterConnection("localhost", 9333);
            master.Start();
            builder.RegisterInstance(master).SingleInstance();
            builder.RegisterType<DistributedFileSystemServiceService>().As<IDistributedFileSystemService>();
            builder.RegisterType<FileMetaData>().As<FileMetaData>();
        }
    }
}