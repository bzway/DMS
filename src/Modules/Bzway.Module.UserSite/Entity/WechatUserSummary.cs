using Bzway.Data.Core;
using System;

namespace Bzway.Framework.Core.Entity
{

    public class WechatUserSummary : BaseEntity
    {
        public string OfficialAccount { get; set; }

        public DateTime RefDateTime { get; set; }
        public WechatUserSource Source { get; set; }
        public int CountOfFollower { get; set; }
        public int CountOfUnfollower { get; set; }
    }

    public enum WechatUserSource
    {
        其他 = 0,
        公众号搜索 = 1,
        名片分享 = 17,
        扫描二维码 = 30,
        图文页右上角菜单 = 43,
        /// <summary>
        /// （在支付完成页）
        /// </summary>
        支付后关注 = 51,
        图文页内公众号名称 = 57,
        公众号文章广告 = 75,
        朋友圈广告 = 78,

    }
}