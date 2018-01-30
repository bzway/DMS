using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;

namespace Bzway.Framework.Application
{
    public class UserIdentity : ClaimsIdentity
    {
        [JsonProperty("i")]
        public string Id { get; set; }
        [JsonProperty("n")]
        public string NickName { get; set; }
        [JsonProperty("r")]
        public string Roles { get; set; }
        [JsonProperty("l")]
        public int Language { get; set; }

        [JsonProperty("v")]
        public int Version { get; set; }
        //[JsonIgnore]
        //public string AuthenticationType
        //{
        //    get { return "Token"; }
        //}
        [JsonIgnore]
        public override bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Id); }
        }

    }
}