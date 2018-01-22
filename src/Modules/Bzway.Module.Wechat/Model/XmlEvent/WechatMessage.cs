using Bzway.Common.Utility;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bzway.Module.Wechat.Model
{   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatTextEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }
        public string Content { get; set; }
    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatImageEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }
        public string PicUrl { get; set; }

        /// <summary>
        /// 通过素材管理接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { get; set; }

    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatVoiceEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }

        public string Format { get; set; }

        /// <summary>
        /// 通过素材管理接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }


    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatVideoEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }
        /// <summary>
        /// 通过素材管理接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }

    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatShortVideoEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }

        /// <summary>
        /// 通过素材管理接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }

    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatLocationEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public string Scale { get; set; }
        public string Label { get; set; }
    }

   
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatLinkEventMessage : WechatMessageBase
    {
        public long MsgId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
  