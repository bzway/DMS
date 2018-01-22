using System;
using System.IO;
using System.Text;

namespace Bzway.Server.Core
{
    public static class HttpBuilder
    {
        public static void Write(this Stream stream, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static string Readline(this Stream stream)
        {
            int next_char;
            StringBuilder data = new StringBuilder();
            while (true)
            {
                next_char = stream.ReadByte();
                if (next_char == '\n')
                {
                    break;
                }
                if (next_char == '\r')
                {
                    continue;
                }
                if (next_char == -1)
                {
                    continue;
                };
                data.Append(Convert.ToChar(next_char));
            }
            return data.ToString();
        }
        /// <summary>
        /// A Stream with the request body, if any.Stream.Null MAY be used as a placeholder if there is no request body.See Request Body.
        /// </summary>
        public const string RequestBody = "owin.RequestBody";
        /// <summary>
        /// An IDictionary<string, string[]> of request headers.See Headers.
        /// </summary>
        public const string RequestHeaders = "owin.RequestHeaders";
        /// <summary>
        /// A string containing the HTTP request method of the request (e.g., "GET", "POST").
        /// </summary>
        public const string RequestMethod = "owin.RequestMethod";
        /// <summary>
        /// A string containing the request path.The path MUST be relative to the "root" of the application delegate. See Paths.
        /// </summary>
        public const string RequestPath = "owin.RequestPath";
        /// <summary>
        /// A string containing the portion of the request path corresponding to the "root" of the application delegate; see Paths.
        /// </summary>
        public const string RequestPathBase = "owin.RequestPathBase";
        /// <summary>
        /// A string containing the protocol name and version (e.g. "HTTP/1.0" or "HTTP/1.1").
        /// </summary>
        public const string RequestProtocol = "owin.RequestProtocol";
        /// <summary>
        /// A string containing the query string component of the HTTP request URI, without the leading " ? " (e.g., "foo=bar&amp;baz=quux"). The value may be an empty string.
        /// </summary>
        public const string RequestQueryString = "owin.RequestQueryString";
        /// <summary>
        /// A string containing the URI scheme used for the request (e.g., "http", "https"); see URI Scheme.
        /// </summary>
        public const string RequestScheme = "owin.RequestScheme";


        /// <summary>
        /// A Stream used to write out the response body, if any.See Response Body.
        /// </summary>
        public const string ResponseBody = "owin.ResponseBody";
        /// <summary>
        /// An IDictionary<string, string[]> of response headers. See Headers.
        /// </summary>
        public const string ResponseHeaders = "owin.ResponseHeaders";
        /// <summary>
        /// An optional int containing the HTTP response status code as defined in RFC 2616 section 6.1.1. The default is 200.
        /// </summary>
        public const string ResponseStatusCode = "owin.ResponseStatusCode";
        /// <summary>
        /// An optional string containing the reason phrase associated the given status code.If none is provided then the server SHOULD provide a default as described in RFC 2616 section 6.1.1
        /// </summary>
        public const string ResponseReasonPhrase = "owin.ResponseReasonPhrase";
        /// <summary>
        /// An optional string containing the protocol name and version(e.g. "HTTP/1.0" or "HTTP/1.1"). If none is provided then the "owin.RequestProtocol" key's value is the default.
        /// </summary>
        public const string ResponseProtocol = "owin.ResponseProtocol";

    }
}