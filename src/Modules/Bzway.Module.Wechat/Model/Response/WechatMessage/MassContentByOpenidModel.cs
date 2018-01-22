using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class MassContentByOpenidModel
    {
        public Array touser { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public mpnews mpnews { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public text text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public voice voice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public image image { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public mpvideo mpvideo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public wxcard wxcard { get; set; }

        public string msgtype { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
