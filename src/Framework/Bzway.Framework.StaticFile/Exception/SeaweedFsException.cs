 

namespace Bzway.Framework.DistributedFileSystemClient.Exception
{
    public class SeaweedFsException : System.InvalidOperationException
    {
        public SeaweedFsException(string message)
            : base (message)
        { }

        public SeaweedFsException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
