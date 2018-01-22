
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bzway.Server.Core
{
    public enum HttpStatusCode
    {
        Continue = 100,
        Ok = 200,
        Created = 201,
        Accepted = 202,
        MovedPermanently = 301,
        Found = 302,
        NotModified = 304,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        InternalServerError = 500
    }

    public class HttpResponse
    {
        private readonly Stream stream;
        public HttpResponse(Stream stream)
        {
            this.Headers = new Dictionary<string, string>();
            this.StatusCode = "200";
            this.ReasonPhrase = "";
            this.stream = stream;
        }
        public string StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Content { get; set; }

        public Dictionary<string, string> Headers { get; set; }


        public void AppendContent(string content, Encoding encoding = null)
        {
            if (this.Content == null)
            {
                this.Content = content;
            }
            else
            {
                this.Content += content;
            }
        }


        // informational only tostring...
        public override string ToString()
        {
            return string.Format("HTTP status {0} {1}", this.StatusCode, this.ReasonPhrase);
        }

        public void WriteResponse()
        {
            if (this.Content == null)
            {
                this.Content = string.Empty;
            }
            // default to text/html content type
            if (!this.Headers.ContainsKey("Content-Type"))
            {
                this.Headers["Content-Type"] = "text/html";
            }

            this.Headers["Content-Length"] = this.Content.Length.ToString();

            stream.Write(string.Format("HTTP/1.0 {0} {1}\r\n", this.StatusCode, this.ReasonPhrase));
            stream.Write(string.Join("\r\n", this.Headers.Select(x => string.Format("{0}: {1}", x.Key, x.Value))));
            stream.Write("\r\n\r\n");
            var buffer = Encoding.UTF8.GetBytes(this.Content);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Dispose();
        }
    }
}
