using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WeChatGetUserListResponseModel : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int total { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int count { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Openids data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string next_openid { get; set; }
        public class Openids
        {
            public List<string> openid { get; set; }
        }
    }




}
