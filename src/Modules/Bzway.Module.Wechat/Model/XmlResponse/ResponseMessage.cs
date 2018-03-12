using Bzway.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Bzway.Module.Wechat
{
    public class ResponseMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string MsgType { get; set; }

        [XmlIgnore]
        public DateTime CreateTime { get; set; }
        [XmlElement("CreateTime")]
        public double CreateTimeWechat
        {
            get
            {
                return DateTimeHelper.GetBaseTimeValue(this.CreateTime);
            }
            set
            {
                this.CreateTime = DateTimeHelper.GetEpochDateTime(value);
            }
        }
        public string ToXMLString()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
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