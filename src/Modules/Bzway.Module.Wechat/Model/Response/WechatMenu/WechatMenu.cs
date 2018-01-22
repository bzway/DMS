using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
namespace Bzway.Module.Wechat.Model
{

    [JsonObject]
    public class WechatMenuModel
    {
        public WechatMenuModel()
        {
            this.sub_button = new List<WechatMenuModel>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string key { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string media_id { get; set; }
        [JsonProperty]
        public List<WechatMenuModel> sub_button { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [JsonObject]
    public class WechatButtonMenu
    {
        public WechatButtonMenu()
        {
            this.button = new List<WechatMenuModel>();
        }
        public List<WechatMenuModel> button { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [JsonObject]
    public class WechatTopMenu
    {
        public WechatButtonMenu menu { get; set; }
    }

    [JsonObject]
    public class WechatMenuResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errcode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errmsg { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string menuid { get; set; }
    }


    [JsonObject]
    public class WechaCustomtMenuResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string menuid { get; set; }
    }

    [JsonObject]
    public class WechatCustomButtonMenu
    {
        public WechatCustomButtonMenu()
        {
            this.button = new List<WechatMenuModel>();
        }
        public List<WechatMenuModel> button { get; set; }

        public WechatCustomMenu matchrule { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [JsonObject]
    public class WechatCustomMenu
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string group_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sex { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string province { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string client_platform_type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string language { get; set; }
    }
}
