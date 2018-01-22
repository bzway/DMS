using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    public  class WechatGetMenuResponseModel
    {
        public List<BaseButton> button { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 所有按钮基类
        /// </summary>
        public class BaseButton
        {
            public string name { get; set; }
            public string type { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string key { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string url { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string media_id { get; set; }
            [JsonProperty]
            public List<BaseButton> sub_button { get; set; }
        }
    }
}