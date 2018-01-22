using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace Bzway.Module.Wechat.Model
{
    public class WechatGroup : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int count { get; set; }
    }
    public class WechatGroupGroup : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WechatGroup group { get; set; }
    }

    public class WechatGroupList : WechatBaseResponseModel
    {
        [JsonProperty]
        public List<WechatGroup> groups { get; set; }
    }

    public class MemberGroup
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string openid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string groupid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string to_groupid { get; set; }
    }

    public class BatchMoveMemberGroup
    {
        [JsonProperty]
        public List<string> openid_list { get; set; }

        [JsonProperty]
        public string to_groupid { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BatchGetUserInfo
    {
        public List<BatchUserList> user_list { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BatchUserList
    {
        public string openid { get; set; }
        public string lang { get; set; }
    }
}
