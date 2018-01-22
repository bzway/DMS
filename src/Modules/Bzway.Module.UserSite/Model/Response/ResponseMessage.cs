using System;
using System.Collections.Generic;
using System.Xml;

using System.Xml.Serialization;
using Bzway.Utility;
namespace Bzway.Framework.Core.Wechat.Models
{
    public class ResponseMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string MsgType { get; set; }

        [XmlIgnore]
        public DateTime CreateTime { get; set; }
        [XmlElement("CreateTime")]
        public long CreateTimeWechat
        {
            get
            {
                return DateTimeHelper.GetWeixinDateTime(this.CreateTime);
            }
            set
            {
                this.CreateTime = DateTimeHelper.GetDateTimeFromXml(value);
            }
        }
        public string ToXMLString()
        {
            return SerializationHelper.SerializationToSoap(this);
        }

    }
    [XmlRoot("xml")]
    public class ResponseTextMessage : ResponseMessage
    {
        public string Content { get; set; }
        public ResponseTextMessage()
        {
            MsgType = "text";
        }
    }

    [XmlRoot("xml")]
    public class ResponseImageMessage : ResponseMessage
    {
        public ResponseImageMessage()
        {
            this.MsgType = "image";
        }
        public string MediaId { get; set; }
    }
    [XmlRoot("xml")]
    public class ResponseVoiceMessage : ResponseMessage
    {
        public ResponseVoiceMessage()
        {
            this.MsgType = "voice";
        }
        public string MediaId { get; set; }
    }
    [XmlRoot("xml")]
    public class ResponseVideoMessage : ResponseMessage
    {
        public ResponseVideoMessage()
        {
            this.MsgType = "video";
        }
        public string MediaId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }
    [XmlRoot("xml")]
    public class ResponseMusicMessage : ResponseMessage
    {
        public ResponseMusicMessage()
        {
            this.MsgType = "music";
        }

        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MusicUrl { get; set; }
        public string HQMusicUrl { get; set; }
        public string ThumbMediaId { get; set; }
    }
    [XmlRoot("xml")]
    public class ResponseNewsMessage : ResponseMessage
    {
        public ResponseNewsMessage()
        {
            this.MsgType = "news";
        }
        public int ArticleCount
        {
            get { return this.Articles.Count; }
            set { }
        }
        [XmlArrayItem("item")]
        public List<Article> Articles { get; set; }

    }
    [XmlRoot("xml")]
    public class ResponseCustomerServiceMessage : ResponseMessage
    {
        public ResponseCustomerServiceMessage()
        {
            this.MsgType = "transfer_customer_service";
        }
    }
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PicUrl { get; set; }
    }

}