using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class MassContentByGroupidModel
    {
        public filter filter { get; set; }

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


    public class filter
    {
        public bool is_to_all { get; set; }
        public string group_id { get; set; }
    }

    public class mpnews
    {
        public string media_id { get; set; }
    }

    public class text
    {
        public string content { get; set; }
    }

    public class voice
    {
        public string media_id { get; set; }

    }

    public class image
    {
        public string media_id { get; set; }
    }

    public class mpvideo
    {
        public string media_id { get; set; }
    }

    public class wxcard
    {
        public string card_id { get; set; }
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string card_ext { get; set; }
    }   
}
