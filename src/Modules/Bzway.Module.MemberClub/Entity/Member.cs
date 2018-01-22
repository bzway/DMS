using Bzway.Data.Core;
using System;

namespace Bzway.Module.MemberClub.Entity
{
    public class Member : EntityBase
    {
        public string Name { get; set; }

        public string UserID { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public GenderType Gender { get; set; }
    }
    public enum GenderType
    {
        Male,
        Female,
        Unknow,
    }
}