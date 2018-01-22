using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bzway.Server.Core
{
    public partial class HttpContext : IHttpContext
    {
        private readonly ConcurrentDictionary<string, object> dict;
        private readonly Dictionary<string, Func<object>> func;
        private readonly TcpClient client;

        public HttpContext(TcpClient client)
        {
            this.dict = new ConcurrentDictionary<string, object>();
            this.func = new Dictionary<string, Func<object>>();
            this.client = client;
            var stream = client.GetStream();
            func.Add(HttpBuilder.RequestBody, () =>
            {
                return stream;
            });
            func.Add(HttpBuilder.ResponseBody, () =>
            {
                return new MemoryStream();
            });
            var reqeust = this.GetRequest(stream);
            func.Add(HttpBuilder.RequestMethod, () =>
            {
                return reqeust.Method;
            });
        }
        public object Get(string key)
        {
            if (this.dict.ContainsKey(key))
            {
                return this.dict[key];
            }
            if (this.func.ContainsKey(key))
            {
                this.dict.TryAdd(key, func[key]());
                return dict[key];
            }
            return null;
        }

        public T Get<T>(string key)
        {
            return (T)this.Get(key);
        }
        public void Set(string key, object value)
        {
            this.dict[key] = value;
        }
        private HttpRequest GetRequest(Stream inputStream)
        {
            //Read Request Line
            string request = inputStream.Readline();

            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            string method = tokens[0].ToUpper();
            string url = tokens[1];
            string protocolVersion = tokens[2];

            //Read Headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string line;
            while ((line = inputStream.Readline()) != null)
            {
                if (line.Equals(""))
                {
                    break;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                string name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;
                }

                string value = line.Substring(pos, line.Length - pos);
                headers.Add(name, value);
            }

            string content = null;
            if (headers.ContainsKey("Content-Length"))
            {
                int totalBytes = Convert.ToInt32(headers["Content-Length"]);
                int bytesLeft = totalBytes;
                byte[] bytes = new byte[totalBytes];

                while (bytesLeft > 0)
                {
                    byte[] buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                    int n = inputStream.Read(buffer, 0, buffer.Length);
                    buffer.CopyTo(bytes, totalBytes - bytesLeft);

                    bytesLeft -= n;
                }

                content = Encoding.ASCII.GetString(bytes);
            }


            return new HttpRequest(this)
            {
                Method = method,
                Url = url,
                Headers = headers,
                Content = content
            };
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
    /*
    public partial class HttpContextTest : IDictionary<string, object>
    {
       public object this[string key]
       {
           get
           {
               return this.dict[key];
           }
           set
           {
               this.dict[key] = value;
           }
       }

       public int Count
       {
           get
           {
               return this.dict.Count;
           }
       }

       public bool IsReadOnly
       {
           get
           {
               return false;
           }
       }

       public ICollection<string> Keys
       {
           get
           {
               return this.dict.Keys;
           }
       }

       public ICollection<object> Values
       {
           get
           {
               return this.dict.Values;
           }
       }

       public void Add(KeyValuePair<string, object> item)
       {
           this.dict.Add(item.Key, item.Value);
       }

       public void Add(string key, object value)
       {

           this.dict.Add(key, value);
       }

       public void Clear()
       {
           this.dict.Clear();
       }

       public bool Contains(KeyValuePair<string, object> item)
       {
           return this.dict.Contains(item);
       }

       public bool ContainsKey(string key)
       {
           return this.dict.ContainsKey(key);
       }

       public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
       {

       }

       public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
       {
           return this.dict.GetEnumerator();
       }

       public bool Remove(KeyValuePair<string, object> item)
       {
           return this.dict.Remove(item.Key);
       }

       public bool Remove(string key)
       {
           return this.dict.Remove(key);
       }

       public bool TryGetValue(string key, out object value)
       {
           return this.dict.TryGetValue(key, out value);
       }

       IEnumerator IEnumerable.GetEnumerator()
       {
           return this.dict.GetEnumerator();
       }
   }
    */
}