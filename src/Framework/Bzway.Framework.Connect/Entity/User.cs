using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Connect.Entity
{
    public class User : EntityBase
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public GenderType Gender { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdCardNo { get; set; }
        public GradeType Grade { get; set; }
        public string Birthday { get; set; }
        public bool IsLunarBirthday { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedTime { get; set; }
        public bool IsConfirmed { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Distinct { get; set; }
        public string Roles { get; set; }
    }

}
