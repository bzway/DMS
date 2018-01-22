using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class WechatCreateOtherMaterialResponseModel : Module.Wechat.WechatBaseResponseModel
    {
        public string media_id { get; set; }
        public string url { get; set; }
    }
}