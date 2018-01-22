using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bzway.Module.Wechat.Model
{
    public class WechatGetUserCumulateResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<usercumulatelist> list { get; set; }
    }

    public class usercumulatelist
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ref_date { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string user_source { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int new_user { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int cancel_user { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int cumulate_user { get; set; }
    }
}
