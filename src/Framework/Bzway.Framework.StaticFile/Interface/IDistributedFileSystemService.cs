using System.IO;
using Bzway.Framework.DistributedFileSystemClient.Core.Http;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public interface IDistributedFileSystemService
    {
        DistributedFileInfo CreateOrUpdateFile(string filePath, Stream stream);
        DirectoryContents ListDirectoryContent(string directoryPath);
        DistributedFileStream GetFileStream(string filePath);
    }
}