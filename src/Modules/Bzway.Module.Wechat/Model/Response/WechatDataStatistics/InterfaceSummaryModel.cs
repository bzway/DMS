using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatGetInterfaceSummaryResponseModel
    {
        public List<InterfaceSummaryList> list { get; set; }
    }

    public class InterfaceSummaryList
    {
        [JsonProperty]
        public string ref_date { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ref_hour { get; set; }
        [JsonProperty]
        public int callback_count { get; set; }
        [JsonProperty]
        public int fail_count { get; set; }
        [JsonProperty]
        public int total_time_cost { get; set; }
        [JsonProperty]
        public int max_time_cost { get; set; }
        

    }
}
