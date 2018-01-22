using Bzway.Common.Utility;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bzway.Module.Wechat.Model
{
    public abstract class WechatMessageBase
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）	  
        /// </summary>
        public virtual string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号	
        /// </summary>
        public virtual string FromUserName { get; set; }


        [XmlIgnore]
        public WechatMessageType MessageType { get; set; }

        [XmlElement("MsgType")]
        public string WechatMsgType
        {
            get
            {
                return Enum.GetName(typeof(WechatMessageType), this.MessageType);
            }
            set
            {
                this.MessageType = (WechatMessageType)Enum.Parse(typeof(WechatMessageType), value);
            }
        }


        [XmlIgnore]
        public DateTime CreateTime { get; set; }
        [XmlElement("CreateTime")]
        public long WechatCreateTime
        {
            get
            {
                return DateTimeHelper.GetBaseTimeValue(this.CreateTime);
            }
            set
            {
                this.CreateTime = DateTimeHelper.ConvertToBaseTime(value);
            }
        }
    }
}