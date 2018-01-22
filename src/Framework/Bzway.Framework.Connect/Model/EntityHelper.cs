using Bzway.Common.Utility;

namespace Bzway.Framework.Connect.Entity
{
    public enum GenderType
    {
        Unkonw = 0,
        Male = 1,
        Female = -1,
    }
    public enum PointType
    {
        Award = 1,
        Transfer = 2,
        Expiry = 3,
        Spend = 4,
    }
    public enum GradeType
    {
        Crystal = 0,
        Sliver = 1,
        Golden = 2,
        Black = 3,
    }
    public enum PhoneType
    {
        MobilePhone,
        Fax,
        Telphone,
    }
    #region resource

    public partial class ViewModelResource
    {
        public static string IsShowMenu { get { return "IsShowMenu".Localize("Member"); } }
        public static string IsShowSideBar { get { return "IsShowSideBar".Localize("Member"); } }
        public static string LoginName { get { return "LoginName".Localize("Member"); } }
        public static string Password { get { return "Password".Localize("Member"); } }
        public static string Remember { get { return "Remember".Localize("Member"); } }
        public static string Province { get { return "Province".Localize("Member"); } }
        public static string Distinct { get { return "Distinct".Localize("Member"); } }
        public static string Country { get { return "Country".Localize("Member"); } }
        public static string Name { get { return "Name".Localize("Member"); } }
        public static string LogoUrl { get { return "LogoUrl".Localize("Member"); } }
        public static string IsLunarBirthday { get { return "Is LunarBirthday".Localize("Member"); } }
        public static string DatabaseName { get { return "DatabaseName".Localize("Member"); } }
        public static string ConnectionString { get { return "ConnectionString".Localize("Member"); } }
        public static string IsShowLogo { get { return "IsShowLogo".Localize("Member"); } }
        public static string FaviconUrl { get { return "FaviconUrl".Localize("Member"); } }
        public static string Caption { get { return "Caption".Localize("Member"); } }
        public static string Description { get { return "Description".Localize("Member"); } }
        public static string Domains { get { return "Domains".Localize("Member"); } }
        public static string TimeZone { get { return "TimeZone".Localize("Member"); } }

        public static string IsLocked { get { return "Is Locked".Localize("Member"); } }
        public static string Birthday { get { return "Birthday".Localize("Member"); } }
        public static string ProviderName { get { return "ProviderName".Localize("Member"); } }
        public static string City { get { return "City".Localize("Member"); } }
        public static string CardNumber { get { return "Card Number".Localize("Member"); } }
        public static string Gender { get { return "Gender".Localize("Member"); } }
        public static string MobileNumber { get { return "Mobile Number".Localize("Member"); } }
        public static string Email { get { return "Email".Localize("Member"); } }
        public static string Grade { get { return "Grade".Localize("Member"); } }
        public static string IsUsed { get { return "Is Used".Localize("Member"); } }
        public static string NickName { get { return "Nick ProviderName".Localize("Member"); } }
        public static string Roles { get { return "Roles".Localize("Member"); } }
        public static string IsConfirmed { get { return "Is Confirmed".Localize("Member"); } }
        public static string LockedTime { get { return "Locked Time".Localize("Member"); } }
    }

    #endregion

}
