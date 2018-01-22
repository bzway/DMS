using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    /// <summary>
    /// 图片、语音、视频
    /// </summary>
    public class WechatBatchGetMaterialResultModel : WechatBaseResponseModel
    {
        public List<media> item { get; set; }
        public int total_count { get; set; }
        public int item_count { get; set; }

        public class media
        {
            public string media_id { get; set; }
            public string update_time { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }
    }
    /// <summary>
    /// 永久图文消息素材
    /// </summary>
    public class WechatBatchGetNewsMaterialResultModel : WechatBaseResponseModel
    {
        public List<media> item { get; set; }
        public int total_count { get; set; }
        public int item_count { get; set; }

        public class media
        {
            public string media_id { get; set; }

            public content content { get; set; }
            public string update_time { get; set; } 
        }

        public class content
        {
            public List<news_item> news_item { get; set; }
        }
        public class news_item {  
            public string title { get; set; }
            public string thumb_media_id { get; set; }
            public string show_cover_pic { get; set; }
            public string author { get; set; }
            public string digest { get; set; }
            public string content { get; set; }
            public string url { get; set; }
            public string  content_source_url { get; set; } 
        }
    }
}