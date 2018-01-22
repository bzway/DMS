using Bzway.Module.Wechat.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatCardModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CCcard card { get; set; }
        public partial class CCcard
        {
            public string card_type { get; set; }
            public groupon groupon { get; set; }
        }

        public class groupon
        {
            public base_info base_info { get; set; }
            public string deal_detail { get; set; }
        }
    }



    public class base_info
    {
        public string logo_url { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string code_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string brand_name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub_title { get; set; }

        public string color { get; set; }

        public string notice { get; set; }

        public string description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string service_phone { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public date_info date_info { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public sku sku { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int get_limit { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool use_custom_code { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool bind_openid { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool can_share { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool can_give_friend { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Array location_id_list { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url_name { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string custom_url_sub_title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string promotion_url_name { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string promotion_url { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string source { get; set; }
    }

    public class sku
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int quantity { get; set; }
    }

    public class date_info
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int begin_timestamp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int end_timestamp { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int fixed_term { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int fixed_begin_term { get; set; }
    }
    public class BaseInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public base_info base_info { get; set; }
    }

    public class QRCodeCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string openid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool is_unique_code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int outer_id { get; set; }
    }

    public class OneCardActionInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QRCodeCard card { get; set; }
    }

    public class SimpleCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string code { get; set; }
    }

    public class SimpleCardList
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SimpleCard> card_list { get; set; }
    }

    public class MultipleCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SimpleCardList multiple_card { get; set; }
    }

    public class MultipleCardActionInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MultipleCard action_info { get; set; }
    }

    public class MultipleQrCodeCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string action_name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MultipleCardActionInfo action_info { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class WechatGetQRCodeCardResponseModel : WechatCreateQRCodeTicketResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string show_qrcode_url { get; set; }
    }

    public class CardUpdateModel
    {
        public string card_id { get; set; }
        public member_card member_card { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class member_card
    {
        public base_info base_info { get; set; }
        public string bonus_cleared { get; set; }
        public string bonus_rules { get; set; }
        public string prerogative { get; set; }
    }

}
