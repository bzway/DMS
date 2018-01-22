using System;
using System.Text;
using Autofac;
using Bzway.Database.Core;
using Bzway.Framework.Application.Entity;
using Bzway.Framework.Application;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Security.Principal;

namespace Bzway.Module.MemberClub
{

    public class MemberService : BaseService<MemberService>, IMemberService
    {
        #region ctor 
        public MemberService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user) : base(loggerFactory, tenant, user)
        {
        }
        #endregion

        //public UserCard FindoCardByID(string id)
        //{
        //    return this.db.Entity<UserCard>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
        //}
        //public void CreateUserCard(UserCard entity)
        //{
        //    db.Entity<UserCard>().Insert(entity);
        //}

        //public void UpdateCard(UserCard model)
        //{
        //    var entity = db.Entity<UserCard>().Query().Where(m => m.UUID, model.UUID, CompareType.Equal).First();
        //    if (entity == null)
        //    {
        //        return;
        //    }
        //    entity.CardGrade = model.CardGrade;
        //    entity.CardName = model.CardName;
        //    entity.CardNumber = model.CardNumber;
        //    entity.IsUsed = model.IsUsed;
        //    entity.ValidateCode = model.ValidateCode;
        //    db.Entity<UserCard>().Update(entity);
        //}

        //public void DeleteCard(string id)
        //{
        //    var entity = db.Entity<UserCard>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
        //    if (entity == null)
        //    {
        //        return;
        //    }
        //    db.Entity<UserCard>().Delete(entity);
        //}
        //public void CreateAccount(string email, string phoneNumber, DynamicEntity data)
        //{

        //    var userEmail = userService.VerifyEmail(email);
        //    var userPhone = userService.VerifyPhone(phoneNumber);
        //    dynamic enity = data;
        //    if (userEmail == null && userPhone == null)
        //    {
        //        #region Create One
        //        var userID = Guid.NewGuid().ToString("N");
        //        using (var db = OpenDatabase.GetDatabase())
        //        {

        //            var user =
        //                //new ViewUser()
        //                new User()
        //                {
        //                    UserName = email,
        //                    Province = enity.Province,
        //                    NickName = enity.NickName,
        //                    Country = enity.Country,
        //                    Name = enity.Name,
        //                    Birthday = enity.Birthday,
        //                    City = enity.City,
        //                    Distinct = enity.Distinct,
        //                    Gender = enity.Gender,
        //                    Grade = GradeType.Crystal,
        //                    IsLocked = false,
        //                    IsLunarBirthday = false,
        //                    LockedTime = null,
        //                    IsConfirmed = false,
        //                    Password = null,
        //                    Roles = string.Empty,
        //                };
        //            db.Entity<User>().Insert(user);
        //            userEmail =
        //                new UserEmail()
        //                //new ViewUserEmail()
        //                {
        //                    ValidateTime = DateTime.UtcNow,
        //                    UserID = userID,
        //                    Email = email,
        //                    IsConfirmed = false,
        //                    ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
        //                };
        //            db.Entity<UserEmail>().Insert(userEmail);
        //            userPhone = new UserPhone()
        //            {
        //                UserID = userID,
        //                IsConfirmed = false,
        //                PhoneNumber = phoneNumber,
        //                Type = PhoneType.MobilePhone,
        //                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
        //            };
        //            db.Entity<UserPhone>().Insert(userPhone);
        //        }
        //        #endregion
        //        using (var db = OpenDatabase.GetDatabase("", "test"))
        //        {
        //            data["ID"] = userID;
        //            db.DynamicEntity(db["Member"]).Insert(data);
        //        }
        //        return;
        //    }

        //    if (userEmail != null && userPhone != null && string.Equals(userPhone.UserID, userEmail.UserID))
        //    {
        //        #region Create One
        //        using (var db = OpenDatabase.GetDatabase("", "test"))
        //        {
        //            data["ID"] = userEmail.UserID;
        //            db.DynamicEntity(db["Member"]).Insert(data);
        //        }
        //        return;

        //        #endregion
        //    }
        //}
    }
}