using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.IO;

namespace Bzway.Framework.StaticFile
{
    public class WebFilePost : IWebFilePost
    {
        public string ContentType { get; set; }

        public string ContentDisposition { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public Stream OpenReadStream { get; set; }

        public void SaveAs(string path) { }
    }
}