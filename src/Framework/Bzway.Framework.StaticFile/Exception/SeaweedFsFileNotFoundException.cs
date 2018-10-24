 
namespace Bzway.Framework.DistributedFileSystemClient.Exception
{
    public class SeaweedFsFileNotFoundException : System.InvalidOperationException
    {
        public SeaweedFsFileNotFoundException(string message)
            : base (message)
        { }

        public SeaweedFsFileNotFoundException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
