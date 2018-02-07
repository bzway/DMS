using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;

namespace Bzway.Framework.Application
{

    public class UserModel
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
    }
    public class UserIdentity : ClaimsIdentity
    {
        public UserModel User { get; set; }
        [JsonIgnore]
        public override bool IsAuthenticated
        {
            get
            {
                if (this.User == null)
                {
                    return false;
                }
                return !string.IsNullOrEmpty(this.User.Id);
            }
        }

    }
}