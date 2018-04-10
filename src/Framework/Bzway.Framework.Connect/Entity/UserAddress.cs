using Bzway.Database.Core;
using System;

namespace Bzway.Framework.Connect.Entity
{
    public class UserAddress : EntityBase
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Distinct { get; set; }
        public string Street { get; set; }
        public string Remark { get; set; }
    }
}
