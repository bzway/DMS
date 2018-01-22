using Bzway.Module.Wechat.Entity;
using System;
using System.Collections.Generic;

namespace Bzway.Module.Wechat
{

    public class WechaGetUserShareResultModel : WechatBaseResponseModel
    {
        public List<UserShare> list { get; set; }

        public class UserShare
        {
            public DateTime ref_date { get; set; }
            public WechatUserShareScene share_scene { get; set; }
            public int share_user { get; set; }
            public int share_count { get; set; }
        }
    }

    public class WechaGetUserShareByHourResultModel : WechatBaseResponseModel
    {
        public List<UserReadByHour> list { get; set; }

        public class UserReadByHour
        {
            public DateTime ref_date { get; set; }
            public int ref_hour { get; set; }
            public WechatUserShareScene share_scene { get; set; }
            public int share_user { get; set; }
            public int share_count { get; set; }

        }
    }
}