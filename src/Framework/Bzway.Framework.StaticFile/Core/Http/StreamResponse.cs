 

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Bzway.Framework.DistributedFileSystemClient.Core.Http
{
    public class StreamResponse
    {
        public StreamResponse(Stream inputStream, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            if (inputStream == null)
                return;

            OutputStream = new MemoryStream();
            inputStream.CopyTo(OutputStream);
            OutputStream.Flush();
            if (OutputStream.CanSeek)
                OutputStream.Seek(0, SeekOrigin.Begin);
        }

        public async Task<Stream> GetInputStream()
        {
            if (OutputStream == null)
                return null;
            var stream = new MemoryStream();
            await OutputStream.CopyToAsync(stream);
            stream.Flush();
            return stream;
        }

        public Stream OutputStream { get; }

        public HttpStatusCode StatusCode { get; }

        public long GetLength()
        {
            if (OutputStream == null)
                return 0;
            return OutputStream.Length;
        }

        public override string ToString()
        {
            return "StreamResponse{" +
                   "byteArrayOutputStream=" + OutputStream +
                   ", statusCode=" + StatusCode +
                   ", length=" + GetLength() +
                   '}';
        }
    }
}
