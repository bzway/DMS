 

namespace Bzway.Framework.DistributedFileSystemClient.Exception
{
    public class SeaweedFsFileUpdateException : System.InvalidOperationException
    {
        public SeaweedFsFileUpdateException(string message)
            : base (message)
        { }

        public SeaweedFsFileUpdateException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
