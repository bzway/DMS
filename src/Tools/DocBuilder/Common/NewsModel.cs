using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Common.Share
{
    public class NewsModel
    {
        public virtual string CreatedBy { get; set; }


        public virtual DateTime CreatedOn { get; set; }

        public virtual string Id { get; set; }

        public virtual string UpdatedBy { get; set; }

        public virtual DateTime UpdatedOn { get; set; }
        /// <summary>
        /// 所属公众号
        /// </summary>
        public string OfficialAccount { get; set; }
        public string MaterialID { get; set; }
        /// <summary>
        /// 微信唯一标识
        /// </summary>
        public string MediaId { get; set; }

        public int SortBy { get; set; }
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息的封面图片素材id（必须是永久mediaID）
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 是否显示封面，0为false，即不显示，1为true，即显示
        /// </summary>
        public bool ShowCoverPicture { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
        /// </summary>
        public string Digest { get; set; }
        /// <summary>
        /// 图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 图文页的URL，或者，当获取的列表是图片素材列表时，该字段是图片的URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 图文消息的原文地址，即点击“阅读原文”后的URL
        /// </summary>
        public string ContentSourceUrl { get; set; }
        /// <summary>
        /// 这篇图文消息素材的最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        public bool IsReleased { get; set; }

    }
}