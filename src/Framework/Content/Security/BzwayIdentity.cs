using OpenData.Utility;
using System;
using System.Security.Principal;
using System.Text;

namespace OpenData.Framework.Common
{
    public class UserIdentity : IIdentity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public int In { get; set; }
        public LockType Locked { get; set; }
        public string Token { get; set; }

        public string AuthenticationType
        {
            get { return "Form"; }
        }

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