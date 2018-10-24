 

namespace Bzway.Framework.DistributedFileSystemClient.Exception
{
    public class SeaweedFsFileDeleteException : System.InvalidOperationException
    {
        public SeaweedFsFileDeleteException(string message)
            : base (message)
        { }

        public SeaweedFsFileDeleteException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }    
}
