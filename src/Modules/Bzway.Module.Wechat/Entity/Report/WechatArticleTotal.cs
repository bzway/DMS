using Bzway.Data.Core;
using System;

namespace Bzway.Module.Wechat.Entity
{

    public class WechatArticleTotal : EntityBase
    {
        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }
        public DateTime SendTime { get; set; }
        public string MessageID { get; set; }
        public string MessageIndex { get; set; }
        public string MaterialID { get; set; }
        public string Title { get; set; }
        public int PageReadUser { get; set; }
        public int PageReadCount { get; set; }
        public int OriginalPageReadUser { get; set; }
        public int OriginalPageReadCount { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
        public int PageFavriateUser { get; set; }
        public int PageFavriateCount { get; set; }

        /// <summary>
        /// 公众号会话阅读人数
        /// </summary>
        public int PageFromSessionReadUser { get; set; }
        /// <summary>
        /// 公众号会话阅读次数
        /// </summary>
        public int PageFromSessionReadCount { get; set; }
        /// <summary>
        /// 历史消息页阅读人数
        /// </summary>
        public int PageFromHistMsgReadUser { get; set; }
        /// <summary>
        /// 历史消息页阅读次数
        /// </summary>
        public int PageFromHistMsgReadCount { get; set; }
        /// <summary>
        /// 朋友圈阅读人数
        /// </summary>
        public int PageFromFeedreadUser { get; set; }
        /// <summary>
        /// 朋友圈阅读次数
        /// </summary>
        public int PageFromFeedreadCount { get; set; }
        /// <summary>
        /// 好友转发阅读人数
        /// </summary>
        public int PageFromFriendsReadUser { get; set; }
        /// <summary>
        /// 好友转发阅读次数
        /// </summary>
        public int PageFromFriendsReadCount { get; set; }
        /// <summary>
        /// 其他场景阅读人数
        /// </summary>
        public int PageFromOtherReadUser { get; set; }
        /// <summary>
        /// 其他场景阅读次数
        /// </summary>
        public int PageFromOtherReadCount { get; set; }
        /// <summary>
        /// 公众号会话转发朋友圈人数
        /// </summary>
        public int FeedShareFromSessionUser { get; set; }
        /// <summary>
        /// 公众号会话转发朋友圈次数
        /// </summary>
        public int FeedShareFromSessionCnt { get; set; }
        /// <summary>
        /// 朋友圈转发朋友圈人数
        /// </summary>
        public int FeedShareFromFeeduser { get; set; }
        /// <summary>
        /// 朋友圈转发朋友圈次数
        /// </summary>
        public int FeedShareFromFeedcnt { get; set; }
        /// <summary>
        /// 其他场景转发朋友圈人数
        /// </summary>
        public int FeedShareFromOtherUser { get; set; }
        /// <summary>
        /// 其他场景转发朋友圈次数
        /// </summary>
        public int FeedShareFromOtherCnt { get; set; }

    }
}