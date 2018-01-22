using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatGetUpStreamMsgResponseModel
    {
        public List<StreamMsgList> list { get; set; }
        public class StreamMsgList
        {
            [JsonProperty]
            public string ref_date { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int ref_hour { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int msg_type { get; set; }
            [JsonProperty]
            public int msg_user { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int msg_count { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int count_interval { get; set; }

        }
    }
}