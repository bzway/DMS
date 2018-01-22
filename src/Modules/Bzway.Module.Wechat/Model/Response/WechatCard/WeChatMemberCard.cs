using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WeChatMemberCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MemberCard card { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class MemberCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public membercard member_card { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public general_coupon general_coupon { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public class membercard
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public baseinfo base_info { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string prerogative { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool auto_activate { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool wx_activate { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool supply_bonus { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string bonus_url { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool supply_balance { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string balance_url { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string bonus_cleared { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string bonus_rules { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string balance_rules { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string activate_url { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public custom_cell custom_cell { get; set; }
        }
    }


    public class general_coupon
    {
        public baseinfo base_info { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string default_detail { get; set; }
    }

    public class custom_cell
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tips { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }

    }
 

    public class ResponsesCard : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool send_check { get; set; }
    }

    public class ConsumeCardResponse : WechatBaseResponseModel
    {
        public cardItem card { get; set; }
        public class cardItem
        {
            public string card_id { get; set; }
        }
        public string openid { get; set; }


    }

    public class DecryptCardResponse : WechatBaseResponseModel
    {
        public string code { get; set; }
    }

    public class modifystock
    {
        public string card_id { get; set; }
        public int increase_stock_value { get; set; }
        public int reduce_stock_value { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class baseinfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string logo_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string code_type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string brand_name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub_title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string color { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string notice { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public skuClass sku { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dateinfo date_info { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public int fixed_term { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public int fixed_begin_term { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? use_custom_code { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? bind_openid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? use_all_locations { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string service_phone { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] location_id_list { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string source { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url_name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url_sub_title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string center_title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string center_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string center_sub_title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string promotion_url_name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string promotion_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string promotion_url_sub_title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int get_limit { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? can_share { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? can_give_friend { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? need_push_on_view { get; set; }

        public class skuClass
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int quantity { get; set; }
        }
        public class dateinfo
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string type { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int fixed_term { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int fixed_begin_term { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int begin_timestamp { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int end_timestamp { get; set; }
        }
    }

}
