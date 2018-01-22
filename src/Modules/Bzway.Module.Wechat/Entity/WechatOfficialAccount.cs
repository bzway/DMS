using Bzway.Data.Core;

namespace Bzway.Module.Wechat.Entity
{
    public class WechatOfficialAccount : EntityBase
    {
        /// <summary>
        /// 请求令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 公众名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公众号OpenID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 服务号
        /// </summary>
        public bool IsServiceAccount { get; set; }
    }
}