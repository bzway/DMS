using Bzway.Database.Core;

namespace Bzway.Framework.Connect.Entity
{
    public class UserCard : EntityBase
    {
        public string UUID { get; set; }
        public string UserID { get; set; }
        public string CardNumber { get; set; }
        public GradeType CardGrade { get; set; }
        public string CardName { get; set; }
        public string ValidateCode { get; set; }
        public bool IsUsed { get; set; }
    }

}
