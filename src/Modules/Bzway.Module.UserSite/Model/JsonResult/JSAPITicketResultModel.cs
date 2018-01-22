namespace Bzway.Framework.Core.Wechat.Models
{

    public class JSAPITicketResultModel : WechatJsonResultModel
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }
}