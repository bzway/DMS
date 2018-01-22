using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{
    public class WechatAddNewsMaterialResultModel : WechatJsonResultModel
    {

        public string media_id { get; set; }


        public string url { get; set; }

    }
    public class WechatUploadNewsImageResultModel : WechatJsonResultModel
    {
        public string url { get; set; }

    }

    public class WechatGetNewsMaterialResultModel : WechatJsonResultModel
    {
        public List<WechatGetNewsMaterialModel> news_item { get; set; }

        public class WechatGetNewsMaterialModel
        {
            public string title { get; set; }


            public string thumb_media_id { get; set; }


            public string thumb_url { get; set; }


            public string show_cover_pic { get; set; }


            public string author { get; set; }


            public string digest { get; set; }


            public string content { get; set; }


            public string url { get; set; }


            public string content_source_url { get; set; }
        }
    }


    public class WechatGetVideoMaterialResultModel : WechatJsonResultModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string down_url { get; set; }
    }
}