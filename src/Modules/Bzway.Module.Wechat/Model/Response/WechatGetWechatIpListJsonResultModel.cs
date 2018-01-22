using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    public class WechatGetWechatIpListJsonResultModel : WechatBaseResponseModel
    {
        public List<string> ip_list { get; set; }
    }

}
