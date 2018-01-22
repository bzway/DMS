using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatCustomSendModel
    {

        public string touser { get; set; }
        public string msgtype { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public text text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public voice voice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public image image { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public video video { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public music music { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public news news { get; set; }
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public wxcard wxcard { get; set; }

    }

    public class video 
    {
        public string media_id { get; set; }
        public string thumb_media_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class music
    {
        public string title { get; set; }
        public string description { get; set; }
        public string musicurl { get; set; }
        public string hqmusicurl { get; set; }
        public string thumb_media_id { get; set; }
    }

    public class news
    {
        public List<article> articles { get; set; }
    }

    public class article
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string picurl { get; set; }
    }
}
