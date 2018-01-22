using Bzway.Database.Core;
using System;
using System.Collections.Generic;
using Bzway.Framework.Application.Entity;
using Microsoft.Extensions.Logging;
using Bzway.Common.Utility;
using System.Linq;

namespace Bzway.Framework.Application
{
    internal class CardLoginService : ILoginService
    {
        #region ctor
        private readonly ILogger<CardLoginService> logger;
        private readonly ISystemDatabase db;
        public CardLoginService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<CardLoginService>();
            this.db = SystemDatabase.GetDatabase();
        }
        #endregion
        public Result<UserProfile> Login(string phoneNumber, string password)
        {
            var user = this.db.Entity<User>().Query().Where(m => m.UserName == phoneNumber).FirstOrDefault();

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