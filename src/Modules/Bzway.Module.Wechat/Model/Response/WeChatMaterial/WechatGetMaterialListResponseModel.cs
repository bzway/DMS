using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{


    public class WechatGetMaterialListResponseModel
    {
        public int total_count { get; set; }
        public int item_count { get; set; }
        public List<Item> item { get; set; }
        public class Item
        {
            [JsonProperty]
            public string media_id { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Content content { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string name { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string update_time { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string url { get; set; }
        }
        public class Content
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
}