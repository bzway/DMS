using Bzway.Data.Core;
using System;

namespace Bzway.Module.Wechat.Entity
{

    public class WechatUserCumulate : EntityBase
    {

        public string OfficialAccount { get; set; }

        public DateTime RefDateTime { get; set; }
        /// <summary>
        /// 关注总人数
        /// </summary>
        public int CountOfFollower { get; set; }
    }
}