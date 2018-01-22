using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    internal class MobileLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<MobileLoginService> logger;
        private readonly ISystemDatabase db;
        public MobileLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<MobileLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }


        #endregion
        public Result<UserProfile> Login(string phoneNumber, string password)
        {
            var userPhone = db.Entity<UserPhone>().Query().Where(m => m.PhoneNumber == phoneNumber).FirstOrDefault();
            User user;
            if (userPhone == null)
            {
                var userId = Guid.NewGuid().ToString("N");
                user = new User()
                {
                    Id = userId,
                    Province = string.Empty,
                    Birthday = string.Empty,
                    City = string.Empty,
                    Country = string.Empty,
                    Distinct = string.Empty,
                    Name = phoneNumber,
                    NickName = phoneNumber,
                    Gender = GenderType.Unkonw,
                    IsConfirmed = false,
                    Grade = GradeType.Crystal,
                    IsLocked = false,
                    IsLunarBirthday = false,
                    LockedTime = null,
                    UserName = phoneNumber,
                    Password = Cryptor.EncryptMD5(password),
                    Roles = string.Empty,

                };
                userPhone = new UserPhone()
                {
                    ValidateTime = DateTime.UtcNow,
                    UserID = userId,
                    PhoneNumber = phoneNumber,
                    Type = PhoneType.MobilePhone,
                    IsConfirmed = false,
                    ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                };


                db.Entity<User>().Insert(user);
                db.Entity<UserPhone>().Insert(userPhone);

                return Result.OK<UserProfile>(new UserProfile()
                {
                    Birthday = user.Birthday,
                    City = user.City,
                    Country = user.Country,
                    Distinct = user.Distinct,
                    Gender = user.Gender,
                    IsConfirmed = user.IsConfirmed,
                    IsLocked = user.IsLocked,
                    IsLunarBirthday = user.IsLunarBirthday,
                    LockedTime = user.LockedTime,
                    Name = user.Name,
                    NickName = user.NickName,
                    Province = user.Province,
                    UserName = user.UserName,
                    Roles = user.Roles.Split(';', ',', '|'),
                });
            }
            user = db.Entity<User>().Query().Where(m => m.Id == userPhone.UserID && !m.IsLocked).FirstOrDefault();
            if (user == null)
            {
                return Result.Fail<UserProfile>(ResultCode.Error);
            }
            if (Cryptor.EncryptMD5(password) == user.Password)
            {
                return Result.OK<UserProfile>(new UserProfile()
                {
                    Birthday = user.Birthday,
                    City = user.City,
                    Country = user.Country,
                    Distinct = user.Distinct,
                    Gender = user.Gender,
                    IsConfirmed = user.IsConfirmed,
                    IsLocked = user.IsLocked,
                    IsLunarBirthday = user.IsLunarBirthday,
                    LockedTime = user.LockedTime,
                    Name = user.Name,
                    NickName = user.NickName,
                    Province = user.Province,
                    UserName = user.UserName,
                    Roles = user.Roles.Split(';', ',', '|'),
                });
            }
            return Result.Fail<UserProfile>(ResultCode.Error);
        }
    }
}