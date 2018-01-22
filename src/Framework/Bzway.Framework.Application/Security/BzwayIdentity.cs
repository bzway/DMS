using Newtonsoft.Json;
using System.Security.Principal;

namespace Bzway.Framework.Application
{
    public class UserIdentity : IIdentity
    {
        [JsonProperty("i")]
        public string Id { get; set; }
        [JsonProperty("n")]
        public string Name { get; set; }
        [JsonProperty("r")]
        public string Roles { get; set; }
        [JsonProperty("s")]
        public LockType Locked { get; set; }

        [JsonProperty("l")]
        public int Language { get; set; }

        [JsonProperty("v")]
        public int Version { get; set; }
        [JsonIgnore]
        public string AuthenticationType
        {
            get { return "Token"; }
        }
        [JsonIgnore]
        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.ID); }
        }
    }
    public enum LockType
    {
        None = 0,
        MobilePhone = 1,
        Email = 2,
        Information = 3,
    }
}