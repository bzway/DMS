using Bzway.Common.Utility;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bzway.Module.Wechat.Model
{
    public enum WechatMessageType
    {
        [Description("text")]
        WechatTextEventMessage = 0,
        [Description("image")]
        Image,
        [Description("voice")]
        Voice,
        [Description("video")]
        Video,
        [Description("shortvideo")]
        ShortVideo,
        [Description("location")]
        Location,
        [Description("link")]
        Link,

        UnSubscribe,
        Subscribe,
        Click,
        View,
        Scan,
    }
}