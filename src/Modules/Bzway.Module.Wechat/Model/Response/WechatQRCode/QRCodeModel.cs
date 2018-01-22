using Newtonsoft.Json;
namespace Bzway.Module.Wechat.Model
{
    public class QRCodeModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int scene_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string scene_str { get; set; }
    }

    public class SceneModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QRCodeModel scene { get; set; }
    }

    public class ActionModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int expire_seconds { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string action_name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SceneModel action_info { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class WechatCreateQRCodeTicketResponseModel : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ticket { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int expire_seconds { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }
    }
    public class WechatCreateFShortUrlResponseModel : WechatBaseResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string action { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string long_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string short_url { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class QRCodeServiceModel
    {
        public string url { get; set; }
        public string qrImage { get; set; }
    }
}
