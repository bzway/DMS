using Bzway.Data.Core;
using System;

namespace Bzway.Framework.Core.Entity
{

    public class WechatUserShare : BaseEntity
    {

        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }
        public WechatUserShareScene Scene { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
    }
    public enum WechatUserShareScene
    {
        好友转发 = 1,
        朋友圈 = 2,
        腾讯微博 = 3,
        其他 = 255

    }
}