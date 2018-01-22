using Bzway.Module.Wechat.Entity;
using System;
using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    public class WechatGetUserSummaryResultModel : WechatBaseResponseModel
    {
        public List<UserSummary> list { get; set; }
        public class UserSummary
        {
            public DateTime ref_date { get; set; }
            public WechatUserSource user_source { get; set; }
            public int new_user { get; set; }
            public int cancel_user { get; set; }

        }
    }
}