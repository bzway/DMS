using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    /// <summary>
    /// 授权Model
    /// </summary>
    public class AuthorizationResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiredIn { get; set; }
    }
   
    public class GrantType
    {
        public static Dictionary<string, Func<string, string, bool>> Rules
        {
            get
            {
                if (list == null)
                {
                    lock (lockObject)
                    {
                        if (list == null)
                        {
                            list = new Dictionary<string, Func<string, string, bool>>();
                            // AppId:应用Id，SecretKey:应用密钥
                            list.Add("client_credential", (appId, SecurityKey) => { return true; });
                            // AppId:微信公众号，SecretKey:OpenId
                            list.Add("wechat_credential", (wechatId, openId) => { return true; });
                        }
                    }
                }

                return list;
            }
        }
        static object lockObject = new object();
        static Dictionary<string, Func<string, string, bool>> list;

    }

}