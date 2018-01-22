using Bzway.Data.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bzway.Module.Wechat.Entity
{
    public class WechatKeyword : EntityBase
    {
        public string OfficialAccount { get; set; }

        [StringLength(100)]
        public string Keyword { get; set; }

        public SearchType SearchType { get; set; }

        [StringLength(100)]
        public string ResponseType { get; set; }

        public int? Probability { get; set; }
        public DateTime? FromTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Content { get; set; }
    }

    public enum ResponseType
    {
        [Description("WeixinText")]
        Text = 0,
        [Description("WeixinImage")]
        Image = 1,
        [Description("WeixinMusic")]
        Music,
        [Description("WeixinVideo")]
        Video,
        [Description("WeixinVoice")]
        Voice,
        [Description("WeixinCSR")]
        CSR,
        [Description("WeixinArticles")]
        Article,
    }

    public enum SearchType
    {
        /// <summary>
        /// 1 = 1
        /// </summary>
        [Description("默认匹配(全包含)")]
        None = 0,
        /// <summary>
        /// x=y
        /// </summary>
        [Description("精确匹配")]
        Equal = 1,
        /// <summary>
        /// x like y + '%'
        /// </summary>
        [Description("开头匹配")]
        StartWith,
        /// <summary>
        /// x like '%' + y
        /// </summary>
        [Description("结尾匹配")]
        EndWith,
        /// <summary>
        /// x like '%' + y + '%'
        /// </summary>
        [Description("关键词包含用户输入匹配")]
        Contain,
        /// <summary>
        /// y like '%' + x + '%'
        /// </summary>
        [Description("用户输入包含关键词匹配")]
        Include,
    }
}
