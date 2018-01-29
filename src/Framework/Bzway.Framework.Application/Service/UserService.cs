﻿using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    public class UserService : IUserService
    {
        #region ctor
        private readonly ILogger<UserService> logger;
        private readonly ISystemDatabase db;
        public UserService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<UserService>();
            this.db = SystemDatabase.GetDatabase();
        }
        #endregion

        public UserEmail VerifyEmail(string email)
        {
            return db.Entity<UserEmail>().Query().Where(m => m.Email == email).First();
        }

        public User FindUserByID(string userID)
        {

            return db.Entity<User>().Query().Where(m => m.Id == userID).First();
        }

        public IEnumerable<UserEmail> FindUserEmailsByUserID(string userID)
        {

            return db.Entity<UserEmail>().Query().Where(m => m.UserID == userID).ToList();
        }

        public UserEmail FindUserEmailByValidationCode(string code)
        {
            return db.Entity<UserEmail>().Query().Where(m => m.ValidateCode == code)
                                     .First();
        }
        public UserPhone FindUserPhoneByValidationCode(string code)
        {
            return db.Entity<UserPhone>().Query().Where(m => m.ValidateCode == code)
                                   .Where(m => m.ValidateTime >= DateTime.UtcNow.AddDays(-1)).First();
        }
        public IEnumerable<UserPhone> FindUserPhonesByUserID(string userID)
        {
            return db.Entity<UserPhone>().Query().Where(m => m.UserID == userID).ToList();
        }

        public UserPhone VerifyPhone(string phoneNumber)
        {
            return db.Entity<UserPhone>().Query().Where(m => m.PhoneNumber == phoneNumber).First();
        }
        public User VerifyUser(string userName, string password)
        {
            password = Cryptor.EncryptMD5(password);
            return db.Entity<User>().Query().Where(m => m.UserName == userName)
                 .Where(m => m.Password == password).First();
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
            var userPhone = new UserPhone()
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
            var userEmail = new UserEmail()
            {
                ValidateTime = DateTime.UtcNow,
                UserID = userId,
                Email = email,
                IsConfirmed = false,
                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
            };

            db.Entity<User>().Insert(user);
            db.Entity<UserEmail>().Insert(userEmail);

            return userEmail;
        }

        public void ConfirmUserEmail(UserEmail userEmail)
        {

            db.Entity<UserEmail>().Update(userEmail);
        }

        public void ConfirmUserPhone(UserPhone userPhone)
        {
            db.Entity<UserPhone>().Update(userPhone);
        }

    }
}