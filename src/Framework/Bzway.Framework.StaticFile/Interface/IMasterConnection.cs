using Bzway.Framework.DistributedFileSystemClient.Core;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public interface IMasterConnection
    {
        Connection GetConnection();
    }
}