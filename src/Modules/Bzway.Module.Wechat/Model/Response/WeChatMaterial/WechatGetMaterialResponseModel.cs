using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatGetNewsMaterialResponseModel
    {
        public List<News_item> news_item { get; set; }
        public class News_item
        {
            [JsonProperty]
            public string title { get; set; }
            [JsonProperty]
            public string thumb_media_id { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string thumb_url { get; set; }
            [JsonProperty]
            public string author { get; set; }
            [JsonProperty]
            public string digest { get; set; }
            [JsonProperty]
            public int show_cover_pic { get; set; }
            [JsonProperty]
            public string content { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string url { get; set; }
            [JsonProperty]
            public string content_source_url { get; set; }
        }
    }

    public class WechatGetOtherMaterialResponseModel
    {
        public List<News_item> news_item { get; set; }
        public class News_item
        {
            [JsonProperty]
            public string title { get; set; }
            [JsonProperty]
            public string thumb_media_id { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string thumb_url { get; set; }
            [JsonProperty]
            public string author { get; set; }
            [JsonProperty]
            public string digest { get; set; }
            [JsonProperty]
            public int show_cover_pic { get; set; }
            [JsonProperty]
            public string content { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string url { get; set; }
            [JsonProperty]
            public string content_source_url { get; set; }
        }
    }
}