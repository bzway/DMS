using Bzway.Data.Core;
using System;

namespace Bzway.Framework.Connect.Entity
{
    public class UserPhone : EntityBase
    {
        public string UserID { get; set; }
        public PhoneType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string ValidateCode { get; set; }
        public DateTime ValidateTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
    
}