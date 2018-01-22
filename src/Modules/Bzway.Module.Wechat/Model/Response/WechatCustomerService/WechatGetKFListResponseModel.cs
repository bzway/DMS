using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatGetKFListResponseModel
    {
        public List<kflist> kf_list { get; set; }
        public class kflist
        {
            public string kf_account { get; set; }
            public string kf_headimgurl { get; set; }
            public string kf_id { get; set; }
            public string kf_nick { get; set; }
            public string kf_wx { get; set; }
            public string invite_status { get; set; }
        }
    }



    public class OpenidToWorkerList
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string Worker { get; set; }
    }

    public class CustomSendModel
    {
        [JsonProperty]
        public string touser { get; set; }
        [JsonProperty]
        public string msgtype { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendText text { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Media_id image { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Media_id voice { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendVideo video { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendMusic music { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendNews news { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Media_id mpnews { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendWxcard wxcard { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomSendCustomservice customservice { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CustomSendText
    {
        public string content { get; set; }
    }

    public class Media_id
    {
        public string media_id { get; set; }
    }

    public class CustomSendVideo
    {
        public string media_id { get; set; }
        public string thumb_media_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class CustomSendMusic
    {
        public string title { get; set; }
        public string description { get; set; }
        public string musicurl { get; set; }
        public string hqmusicurl { get; set; }
        public string thumb_media_id { get; set; }
    }

    public class CustomSendNews
    {
        public List<CustomSendNewsArticle> articles { get; set; }
    }

    public class CustomSendNewsArticle
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string picurl { get; set; }
    }

    public class CustomSendWxcard
    {
        public string card_id { get; set; }
        public string card_ext { get; set; }
    }

    public class CustomSendCustomservice
    {
        public string kf_account { get; set; }
    }

    public class OnLineKFModel
    {
        public List<OnLineKFList> kf_online_list { get; set; }
    }

    public class OnLineKFList
    {
        public string kf_account { get; set; }
        public int status { get; set; }
        public string kf_id { get; set; }
        public int auto_accept { get; set; }
        public int accepted_case { get; set; }
    }

    public class CustomModel
    {
        public string kf_account { get; set; }
        public string nickname { get; set; }

        public string invite_wx { get; set; }
        //public string password { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class KFSessionModel
    {
        public string kf_account { get; set; }
        public string openid { get; set; }
        public string text { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class KFSessionResponseModel
    {
        public string createtime { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string kf_account { get; set; }
    }
}
