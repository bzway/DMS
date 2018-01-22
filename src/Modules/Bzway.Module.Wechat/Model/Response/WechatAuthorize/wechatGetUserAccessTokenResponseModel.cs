using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{

    public class wechatGetUserAccessTokenResponseModel : WechatBaseResponseModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public string expiresIn { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }
        public string openId { get; set; }
        public string scope { get; set; }
        public string unionId { get; set; }
    }
}