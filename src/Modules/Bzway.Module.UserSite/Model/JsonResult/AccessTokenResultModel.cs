namespace Bzway.Framework.Core.Wechat.Models
{
    /// <summary>
    /// access_token请求后的JSON返回格式
    /// </summary>
    public class AccessTokenResultModel : WechatJsonResultModel
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }
}