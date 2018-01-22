using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Application.Entity
{
    public class UserToken : EntityBase
    {
        public string UserID { get; set; }
        public DateTime ExpringTime { get; set; }
        public string AccessToken { get; set; }
    }
}