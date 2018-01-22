using System;
using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{
    public class WechatGetArticleSummaryResultModel : WechatJsonResultModel
    {
        public List<ArticleSummary> list { get; set; }
        public class ArticleSummary
        {
            public DateTime ref_date { get; set; }
            public string msgid { get; set; }
            public string title { get; set; }
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