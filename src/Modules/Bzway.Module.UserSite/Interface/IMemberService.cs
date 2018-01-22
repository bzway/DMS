using Bzway.Data.Core;
using Bzway.Framework.Core.Entity;
namespace Bzway.Framework.Core
{
    public interface IMemberService
    {
        void CreateAccount(string email, string phoneNumber, DynamicEntity data);
        void Import(System.Data.DataSet ds, Site site);
        //Bzway.Business.Model.MemberSearchViewModel SearchMember(Bzway.Business.Model.MemberSearchViewModel model);
    }
}
