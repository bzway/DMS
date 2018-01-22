using System.Collections.Generic;

namespace Bzway.Module.Wechat
{
    public class WechatGetGroupModel : WechatBaseResponseModel
    {
        public List<WechatUserGroup> groups { get; set; }

        public class WechatUserGroup
        {
            public string id { get; set; }
            public string name { get; set; }
            public int count { get; set; }
        }
    }
}