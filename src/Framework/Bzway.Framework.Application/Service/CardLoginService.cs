using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;
using Bzway.Common.Share;

namespace Bzway.Framework.Application
{
    internal class UserLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<UserLoginService> logger;
        private readonly ISystemDatabase db;
        public UserLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<UserLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }
        #endregion

        public Result<UserProfile> Login(string userName, string password, string validateCode)
        {
            var key = "authorize_code:" + validateCode;
            if (!CacheManager.Default.RedisCacheProvider.IsSet(key))
            {
                return Result<UserProfile>.Fail(ResultCode.Error, "验证码错误");
            }
            if (RegexHelper.IsEmail(userName))
            {
                return this.EmailLogin(userName, password, validateCode);
            }
            else if (RegexHelper.IsMobileNumber(userName))
            {
                return this.MobileLogin(userName, password, validateCode);
            }

            return this.CardLogin(userName, password, validateCode);
        }

        private Result<UserProfile> MobileLogin(string phoneNumber, string password, string code)
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

                return Result<UserProfile>.Success(new UserProfile()
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
                return Result<UserProfile>.Fail(ResultCode.Error);
            }
            if (Cryptor.EncryptMD5(password) == user.Password)
            {

                return Result<UserProfile>.Success(new UserProfile()
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
                    Id = user.Id,
                    NickName = user.NickName,
                    Province = user.Province,
                    UserName = user.UserName,
                    Roles = user.Roles.Split(';', ',', '|'),
                });
            }
            return Result<UserProfile>.Fail(ResultCode.Error);
        }
        private Result<UserProfile> EmailLogin(string email, string password, string code)
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
                return Result<UserProfile>.Success(new UserProfile()
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
                return Result<UserProfile>.Fail(ResultCode.Error);
            }
            if (Cryptor.EncryptMD5(password) == user.Password)
            {
                var a = Cryptor.EncryptMD5(password);
                return Result<UserProfile>.Success(new UserProfile()
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
                    Id = user.Id,
                    NickName = user.NickName,
                    Province = user.Province,
                    UserName = user.UserName,
                    Roles = user.Roles.Split(';', ',', '|'),
                });
            }
            return Result<UserProfile>.Fail(ResultCode.Error);
        }
        private Result<UserProfile> CardLogin(string phoneNumber, string password, string code)
        {
            var user = this.db.Entity<User>().Query().Where(m => m.UserName == phoneNumber).FirstOrDefault();

            if (user == null)
            {
                return Result<UserProfile>.Fail(ResultCode.Error);
            }
            if (Cryptor.EncryptMD5(password) == user.Password)
            {
                return Result<UserProfile>.Success(new UserProfile()
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
                    Id = user.Id,
                    NickName = user.NickName,
                    Province = user.Province,
                    UserName = user.UserName,
                    Roles = user.Roles.Split(';', ',', '|'),
                });
            }
            return Result<UserProfile>.Fail(ResultCode.Error);
        }
    }
}