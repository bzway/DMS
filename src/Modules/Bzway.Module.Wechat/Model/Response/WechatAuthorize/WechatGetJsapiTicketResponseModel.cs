using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
namespace Bzway.Module.Wechat.Model
{


    public class WechatGetJsapiTicketResponseModel : WechatBaseResponseModel
    {
        [JsonProperty]
        public string ticket { get; set; }
        [JsonProperty]
        public string expires_in { get; set; }
    }
 

    public class WeChatSDKModel
    {
        public string appId { get; set; }
        public int timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
}