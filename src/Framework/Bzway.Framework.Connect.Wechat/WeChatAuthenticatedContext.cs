using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Claims;
namespace Microsoft.Owin.Security.Weixin
{
    public class WeChatAuthenticatedContext : BaseContext
    {
        public JObject User
        {
            get;
            private set;
        }
        public string AccessToken
        {
            get;
            private set;
        }
        public string Id
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public ClaimsIdentity Identity
        {
            get;
            set;
        }
        public AuthenticationProperties Properties
        {
            get;
            set;
        }
        public WeChatAuthenticatedContext(IOwinContext context, string openId, JObject user, string accessToken) : base(context)
        {
            this.User = user;
            this.AccessToken = accessToken;
            this.Id = openId;
            this.Name = WeChatAuthenticatedContext.PropertyValueIfExists("nickname", user);
        }
        private static string PropertyValueIfExists(string property, IDictionary<string, JToken> dictionary)
        {
            return dictionary.ContainsKey(property) ? dictionary[property].ToString() : null;
        }
    }
}
