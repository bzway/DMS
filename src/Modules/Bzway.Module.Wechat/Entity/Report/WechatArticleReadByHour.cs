using Bzway.Data.Core;
using System;

namespace Bzway.Module.Wechat.Entity
{

    public class WechatArticleReadByHour : EntityBase
    {

        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }

        public WechatUserReadSource Source { get; set; }
        public int PageReadUser { get; set; }
        public int PageReadCount { get; set; }
        public int OriginalPageReadUser { get; set; }
        public int OriginalPageReadCount { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
        public int PageFavriateUser { get; set; }
        public int PageFavriateCount { get; set; }


    }
    public enum WechatUserReadSource
    {
        会话 = 0,
        好友 = 1,
        朋友圈 = 2,
        腾讯微博 = 3,
        历史消息页 = 4,
        其他 = 5,
    }

}