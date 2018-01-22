using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class PoiRequestModel
    {
        public PoiBussiness business { get; set; }
    }

    public class PoiBussiness
    {
        public Poi_base_info base_info { get; set; }
    }

    public class Poi_base_info
    {
        public string sid { get; set; }
        public string business_name { get; set; }
        public string branch_name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string categories { get; set; }
        public int offset_type { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public List<Photo_info> photo_list { get; set; }
        public string recommend { get; set; }
        public string special { get; set; }
        public string introduction { get; set; }
        public string open_time { get; set; }
        public int avg_price { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int available_state { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int update_status { get; set; }

        public class Photo_info
        {
            public string photo_url { get; set; }
        }
    }

    public class PoiResponseModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public PoiBussiness business { get; set; }
    }

    public class PoiListResponseModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public List<PoiBussiness> business_list { get; set; }
        public int total_count { get; set; }
    }

    public class UpdatePoiResponseModel
    {
        public UpdatePoiBussiness business { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public class UpdatePoiBussiness
        {
            public UpdatePoiBaseInfo base_info { get; set; }

            public class UpdatePoiBaseInfo
            {
                public string poi_id { get; set; }
                public string telephone { get; set; }
                public List<Photo_info> photo_list { get; set; }
                public string recommend { get; set; }
                public string special { get; set; }
                public string introduction { get; set; }
                public string open_time { get; set; }
                public int avg_price { get; set; }

                public class Photo_info
                {
                    public string photo_url { get; set; }
                }
            }
        }
    }

    public class PoiCategoryResponseModel
    {
        public Array category_list { get; set; }
    }

    
}
