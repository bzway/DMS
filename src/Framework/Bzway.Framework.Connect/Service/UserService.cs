using Bzway.Data.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Connect.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;

namespace Bzway.Framework.Connect
{
    public class UserService : IUserService
    {
        #region ctor
        private readonly ILogger<UserService> logger;
        private readonly IDatabase db;
        public UserService(ILoggerFactory loggerFactory)
        {
            this.db = OpenDatabase.GetDatabase();
            this.logger = loggerFactory.CreateLogger<UserService>();
        }
        #endregion

        public UserEmail VerifyEmail(string email)
        {
            return db.Entity<UserEmail>().Query().Where(m => m.Email, email, CompareType.Equal).First();
        }

        public User FindUserByID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<User>().Query().Where(m => m.Id, userID, CompareType.Equal).First();
            }
        }

        public IEnumerable<UserEmail> FindUserEmailsByUserID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserEmail>().Query().Where(m => m.UserID, userID, CompareType.Equal).ToList();
            }
        }

        public UserEmail FindUserEmailByValidationCode(string code)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserEmail>().Query().Where(m => m.ValidateCode, code, CompareType.Equal)
                                     //.Where(m => m.ValidateTime, DateTime.UtcNow.AddDays(-1), CompareType.GreaterThanOrEqual)
                                     .First();
            }
        }

        public UserPhone FindUserPhoneByValidationCode(string code)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.ValidateCode, code, CompareType.Equal)
                                     .Where(m => m.ValidateTime, DateTime.UtcNow.AddDays(-1), CompareType.GreaterThanOrEqual).First();
            }
        }

        public IEnumerable<UserPhone> FindUserPhonesByUserID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.UserID, userID, CompareType.Equal).ToList();
            }
        }

        public UserPhone VerifyPhone(string phoneNumber)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.PhoneNumber, phoneNumber, CompareType.Equal).First();
            }
        }
        public User VerifyUser(string userName, string password)
        {
            password = Cryptor.EncryptMD5(password);
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<User>().Query().Where(m => m.UserName, userName, CompareType.Equal)
                    .Where(m => m.Password, password, CompareType.Equal).First();
            }
        }


        public UserPhone RegisterByPhoneNumber(string phoneNumber, string password)
        {
            var userId = Guid.NewGuid().ToString("N");
            var user = new User()
            {
                Id = userId,
                Province = string.Empty,
                Birthday = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                Distinct = string.Empty,
                FamilyName = phoneNumber,
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
            var userPhone = new UserPhone()
            {
                ValidateTime = DateTime.UtcNow,
                UserID = userId,
                PhoneNumber = phoneNumber,
                Type = PhoneType.MobilePhone,
                IsConfirmed = false,
                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
            };

            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Insert(user);
                db.Entity<UserPhone>().Insert(userPhone);
            }
            return userPhone;
        }

        public virtual UserEmail RegisterByEmail(string email, string password)
        {
            var userId = Guid.NewGuid().ToString("N");
            var user = new User()
            {
                Id = userId,
                Province = string.Empty,
                Birthday = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                Distinct = string.Empty,
                FamilyName = email,
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
            var userEmail = new UserEmail()
            {
                ValidateTime = DateTime.UtcNow,
                UserID = userId,
                Email = email,
                IsConfirmed = false,
                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
            };
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Insert(user);
                db.Entity<UserEmail>().Insert(userEmail);
            }
            return userEmail;
        }

        public void ConfirmUserEmail(UserEmail userEmail)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<UserEmail>().Update(userEmail);
            }
        }

        public void ConfirmUserPhone(UserPhone userPhone)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<UserPhone>().Update(userPhone);
            }
        }

    }
}