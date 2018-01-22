using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    internal class EmailLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<EmailLoginService> logger;
        private readonly ISystemDatabase db;
        public EmailLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<EmailLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }
        #endregion 
        public Result<UserProfile> Login(string email, string password)
        {
            var userEmail = this.db.Entity<UserEmail>().Query().Where(m => m.Email == email).FirstOrDefault();
            User user;
            if (userEmail == null)
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
                    Name = email,
                    NickName = email,
                    Gender = GenderType.Unkonw,
                    IsConfirmed = false,
                    Grade = GradeType.Crystal,
                    IsLocked = false,
                    IsLunarBirthday = false,
                    LockedTime = null,
                    UserName = email,
                    Password = Cryptor.EncryptMD5(password),
                    Roles = string.Empty,
                };
                userEmail = new UserEmail()
                {
                    ValidateTime = DateTime.UtcNow,
                    UserID = userId,
                    Email = email,
                    IsConfirmed = false,
                    ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                };

                db.Entity<User>().Insert(user);
                db.Entity<UserEmail>().Insert(userEmail);

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
            user = this.db.Entity<User>().Query().Where(m => m.Id == userEmail.UserID && !m.IsLocked).FirstOrDefault();
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