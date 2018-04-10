using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Connect.Entity
{
    public class UserEmail : EntityBase
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string ValidateCode { get; set; }
        public DateTime ValidateTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}