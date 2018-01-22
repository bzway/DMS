using Newtonsoft.Json;

namespace Bzway.Module.Wechat
{
    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class WechatBaseResponseModel
    {
        [JsonProperty(PropertyName = "errcode")]
        public int errcode { get; set; }
        [JsonProperty(PropertyName = "errmsg")]
        public string errmsg { get; set; }
        [JsonIgnore]
        public bool HasError { get { return this.errcode > 0; } }
    }
}
