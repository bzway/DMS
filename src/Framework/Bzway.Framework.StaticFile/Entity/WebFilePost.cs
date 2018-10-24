using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public class WebFilePost : IWebFilePost
    {
        public string ContentType { get; set; }

        public string ContentDisposition { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public Stream OpenReadStream { get; set; }

        public void SaveAs(string path)
        {
            using (var io = File.OpenWrite(path))
            {
                OpenReadStream.CopyTo(io);
                OpenReadStream.Close();
            }
        }
    }
}