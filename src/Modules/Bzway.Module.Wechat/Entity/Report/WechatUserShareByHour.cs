using Bzway.Data.Core;
using System;

namespace Bzway.Module.Wechat.Entity
{

  
    public class WechatUserShareByHour : EntityBase
    {

        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }
        public WechatUserShareScene Scene { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
    }
}