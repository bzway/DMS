using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Bzway.Framework.StaticFile
{
    public interface IStaticFileService
    {
        IDirectoryContents GetDirectoryContents(string subpath);
        IFileInfo GetFileInfo(string subpath);
        IChangeToken Watch(string filter);
    }
}