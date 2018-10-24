using System.Linq;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using Bzway.Framework.DistributedFileSystemClient.Core.Http;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public struct DistributedFileInfo
    {
        public string ContentType { get; set; }
        public long Length { get; set; }

        public string FileId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public bool IsDirectory { get; set; }

        public string Url { get; set; }
    }

    public class DirectoryContents : List<DistributedFileInfo>
    {
        public DirectoryContents(IEnumerable<DistributedFileInfo> collection) : base(collection)
        {
        }
    }

    public class DistributedFileStream
    {
        public DistributedFileInfo Info { get; set; }
        public StreamResponse Stream { get; set; }
    }
}