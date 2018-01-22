using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{



    public class WechatGetArticleSummaryResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string msgid { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string title { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_count { get; set; }
        }
    }

    public class WechatGetArticleTotalResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string msgid { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string title { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<articledetails> details { get; set; }
            public class articledetails
            {
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public DateTime stat_date { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int target_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int ori_page_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int ori_page_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int share_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int share_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int add_to_fav_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int add_to_fav_count { get; set; }
                public int int_page_from_session_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_session_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_hist_msg_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_hist_msg_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_feed_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_feed_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_friends_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_friends_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_other_read_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int int_page_from_other_read_count { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_session_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_session_cnt { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_feed_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_feed_cnt { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_other_user { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int feed_share_from_other_cnt { get; set; }
            }
        }

    }

    public class WechatGetArticleReadResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string user_source { get; set; }
           
        }
    }

    public class WechatGetArticleReadByHourResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ref_hour { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string user_source { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int int_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ori_page_read_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int add_to_fav_count { get; set; }

        }
    }
    public class WechatGetArticleShareResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string share_scene { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_user { get; set; }
        }
    }

    public class WechatGetArticleShareByHourResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<articlelist> list { get; set; }
        public class articlelist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ref_hour { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string share_scene { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int share_user { get; set; }
        }
    }
}
