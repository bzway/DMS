using Bzway.Framework.Core.Entity;
using System;
using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{

    public class WechatGetUserReadResultModel : WechatJsonResultModel
    {
        public List<UserRead> list { get; set; }

        public class UserRead
        {
            public DateTime ref_date { get; set; }

            public int int_page_read_user { get; set; }
            public int int_page_read_count { get; set; }
            public int ori_page_read_user { get; set; }
            public int ori_page_read_count { get; set; }
            public int share_user { get; set; }
            public int share_count { get; set; }
            public int add_to_fav_user { get; set; }
            public int add_to_fav_count { get; set; }
        }
    }

    public class WechatGetUserReadByHourResultModel : WechatJsonResultModel
    {
        public List<UserReadByHour> list { get; set; }

        public class UserReadByHour
        {
            public DateTime ref_date { get; set; }
            public int ref_hour { get; set; }
            public WechatUserReadSource user_source { get; set; }
            public int int_page_read_user { get; set; }
            public int int_page_read_count { get; set; }
            public int ori_page_read_user { get; set; }
            public int ori_page_read_count { get; set; }
            public int share_user { get; set; }
            public int share_count { get; set; }
            public int add_to_fav_user { get; set; }
            public int add_to_fav_count { get; set; }
        }
    }
}