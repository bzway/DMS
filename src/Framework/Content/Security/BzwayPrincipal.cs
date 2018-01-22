using Microsoft.Owin;
using OpenData.Utility;
using System.Security.Principal;
using System.Linq;
using System.Collections.Generic;

namespace OpenData.Framework.Common
{
    public class BzwayPrincipal : IPrincipal
    {
        private static readonly string SessionKey = "SessionKey";
        private static readonly string PasswordKey = "passwordKey";
        private readonly IDictionary<string, string> cookies;
        private UserIdentity identity;

        public BzwayPrincipal(IDictionary<string, string> cookies)
        {
            this.cookies = cookies;
        }
        public IIdentity Identity
        {
            get
            {
                if (this.identity == null && cookies.ContainsKey(SessionKey))
                {
                    var data = cookies[SessionKey];
                    data = Cryptor.DecryptAES(data, PasswordKey);
                    this.identity = SerializationHelper.DeserializeObjectJson<UserIdentity>(data);
                }
                return this.identity;
            }
        }

        public bool IsInRole(string role)
        {
            return this.identity.Roles.Contains(role);
        }
        public override string ToString()
        {
            return this.identity.Name;
        }
    }

}