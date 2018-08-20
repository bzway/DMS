using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Bzway.Common.Utility
{

    public class SimpleWebToken
    {
        private static readonly Dictionary<string, TokenPolicy> TokenPolicies;

        class TokenPolicy
        {
            private readonly HashAlgorithm algorithm;
            private readonly ISerializePolicy serializePolicy;
            public TokenPolicy(HashAlgorithm algorithm, ISerializePolicy serializePolicy)
            {
                this.algorithm = algorithm;
                this.serializePolicy = serializePolicy;
            }
            public byte[] Hash(byte[] data)
            {
                return this.algorithm.ComputeHash(data);
            }
            public string Serialize(object payload)
            {
                return this.serializePolicy.Serialize(payload);
            }
            public T Deserialize<T>(string payload) where T : new()
            {
                return (T)this.serializePolicy.Deserialize<T>(payload);
            }
        }
        class SerializePolicy : ISerializePolicy
        {
            public string Serialize(object payload)
            {
                return JsonConvert.SerializeObject(payload, Formatting.None);
            }
            public T Deserialize<T>(string payload) where T : new()
            {
                return JsonConvert.DeserializeObject<T>(payload);
            }
        }


        static SimpleWebToken()
        {
            TokenPolicies = new Dictionary<string, TokenPolicy>
            {
                { "RS256", new TokenPolicy(new HMACSHA256(),new SerializePolicy())},
                { "MD5", new TokenPolicy(MD5.Create(),new SerializePolicy())},
                { "HS384", new TokenPolicy(new HMACSHA384(),new SerializePolicy())},
                { "HS512", new TokenPolicy(new HMACSHA512(),new SerializePolicy())},
            };
        }

        public static string Encode(object payload, string policy)
        {
            if (string.IsNullOrEmpty(policy))
            {
                policy = "RS256";
            }
            if (!TokenPolicies.ContainsKey(policy))
            {
                return string.Empty;
            }
            var tokenPolicy = TokenPolicies[policy];

            var encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(policy));
            var payloadString = tokenPolicy.Serialize(payload);
            var encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadString));
            var encodedBody = string.Concat(encodedHeader, ".", encodedPayload);
            var signature = tokenPolicy.Hash(Encoding.UTF8.GetBytes(encodedBody));
            var encodedSignature = Base64UrlEncode(signature);
            return string.Concat(encodedBody, ".", encodedSignature);
        }


        public static object Decode(string token)
        {
            return Decode<object>(token);
        }
        public static T Decode<T>(string token) where T : new()
        {
            var parts = token.Split('.');
            var encodedHeader = parts[0];
            var policy = Encoding.UTF8.GetString(Base64UrlDecode(encodedHeader));
            if (!TokenPolicies.ContainsKey(policy))
            {
                return default(T);
            }
            var tokenPolicy = TokenPolicies[policy];
            var encodedPayload = parts[1];
            var encodedBody = string.Concat(encodedHeader, ".", encodedPayload);
            var signature = tokenPolicy.Hash(Encoding.UTF8.GetBytes(encodedBody));
            var encodedSignature = Base64UrlEncode(signature);
            if (parts[2] != encodedSignature)
            {
                return default(T);
            }
            return tokenPolicy.Deserialize<T>(Encoding.UTF8.GetString(Base64UrlDecode(encodedPayload)));
        }
        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.TrimEnd('='); // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }
        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}