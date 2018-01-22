using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{
    public class WechatGetWechatIpListJsonResultModel : WechatJsonResultModel
    {
        public List<string> ip_list { get; set; }
    }

}
