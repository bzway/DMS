using System;
using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    public class WechatGetUserCumulateResultModel : WechatBaseResponseModel
    {
        public class UserCumulate
        {
            public DateTime ref_date { get; set; }
            public int cumulate_user { get; set; }
        }
        public List<UserCumulate> list { get; set; }
    }
}