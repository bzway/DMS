using System;
using System.Linq;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;

namespace Bzway.Framework.Application
{
    public class UserProfile
    {
        public string Name { get; set; }
        public GenderType Gender { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Distinct { get; set; }
        public string Birthday { get; set; }
        public bool IsLunarBirthday { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedTime { get; set; }
        public bool IsConfirmed { get; set; }
        public IList<string> Roles { get; set; }
        public int Language { get; set; }
        public string Id { get; set; }
    }
}