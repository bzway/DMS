using System.IO;
using Microsoft.AspNetCore.Http;

namespace Bzway.Framework.StaticFile
{
    public interface IWebFilePost
    {
        string ContentDisposition { get; set; }
        string ContentType { get; set; }
        string FileName { get; set; }
        IHeaderDictionary Headers { get; set; }
        long Length { get; set; }
        string Name { get; set; }
        Stream OpenReadStream { get; set; }

        void SaveAs(string path);
    }
}