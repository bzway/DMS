using Bzway.Framework.Core.Entity;
using System.Collections.Generic;
namespace Bzway.Framework.Core
{
    public interface IUserService
    {
        /// <summary>
        /// 根据用户ID得到用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        User FindUserByID(string userID);
        /// <summary>
        /// 根据用户ID得到用户邮箱列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<UserEmail> FindUserEmailsByUserID(string userID);
        /// <summary>
        /// 根据用户ID得到用户电话列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<UserPhone> FindUserPhonesByUserID(string userID);
        UserEmail FindUserEmailByValidationCode(string code);
        UserPhone FindUserPhoneByValidationCode(string code);
        /// <summary>
        /// 通过电子邮箱注册
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserEmail RegisterByEmail(string email, string password);
        /// <summary>
        /// 通过手机号注册
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserPhone RegisterByPhoneNumber(string phoneNumber, string password);
        UserEmail VerifyEmail(string email);
        UserPhone VerifyPhone(string phoneNumber);
        User VerifyUser(string userName, string password);
        void ConfirmUserEmail(UserEmail userEmail);
        void ConfirmUserPhone(UserPhone userPhone);
    }
}
