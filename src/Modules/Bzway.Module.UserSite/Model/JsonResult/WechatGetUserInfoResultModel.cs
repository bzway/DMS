using Bzway.Utility;
using System;

namespace Bzway.Framework.Core.Wechat.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class WechatGetUserInfoResultModel : WechatJsonResultModel
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，拉取不到其余信息
        /// </summary>
        public string subscribe { get; set; }
        /// <summary>
        /// 普通用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 普通用户的昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex { get; set; }


        /// <summary>
        /// 用户所在国家
        /// </summary>

        public string country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 普通用户的头像链接
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 普通用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }


        public string subscribe_time { get; set; }
        public DateTime SubscribeTime()
        {
            return DateTimeHelper.GetDateTimeFromXml(subscribe_time);
        }

        public string unionid { get; set; }

        public string remark { get; set; }

        public string groupid { get; set; }

    }
}
