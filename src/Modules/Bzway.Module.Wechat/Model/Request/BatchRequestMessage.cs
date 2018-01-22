
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Module.Wechat
{
    public class BatchRequestMessage
    {
        public List<string> touser { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
    /// <summary>
    /// 组群发图文消息
    /// </summary>
    public class BatchRequestNewsMessage : BatchRequestMessage
    {
        public BatchRequestNewsMessage(string openIds, string media_id)
        {
            this.touser = openIds.Split(',').ToList();
            this.mpnews = new Media() { media_id = media_id };
        }

        public string msgtype { get { return "mpnews"; } }

        public Media mpnews { get; set; }
        public class Media
        {
            public string media_id { get; set; }
        }
    }


    /// <summary>
    /// 组群发文本消息
    /// </summary>
    public class BatchRequestTextMessage : BatchRequestMessage
    {
        public BatchRequestTextMessage(string openIds, string content)
        {
            this.touser = openIds.Split(',').ToList();
            this.text = new WechatNews() { content = content };
        }
        public string msgtype { get { return "text"; } }

        public WechatNews text { get; set; }

        public class WechatNews
        {
            public string content { get; set; }
        }

    }

    /// <summary>
    /// 组群发语音消息
    /// </summary>
    public class BatchRequestVoiceMessage : BatchRequestMessage
    {
        public BatchRequestVoiceMessage(string openIds, string media_id)
        {
            this.touser = openIds.Split(',').ToList();
            this.voice = new Media() { media_id = media_id };
        }

        public string msgtype { get { return "voice"; } }

        public Media voice { get; set; }
        public class Media
        {
            public string media_id { get; set; }
        }

    }


    /// <summary>
    /// 组群发图片消息
    /// </summary>

    public class BatchRequestImageMessage : BatchRequestMessage
    {
        public BatchRequestImageMessage(string openIds, string media_id)
        {
            this.touser = openIds.Split(',').ToList();
            this.image = new Media() { media_id = media_id };
        }

        public string msgtype { get { return "image"; } }

        public Media image { get; set; }
        public class Media
        {
            public string media_id { get; set; }
        }
    }
    /// <summary>
    /// 发送视频消息
    /// </summary>
    public class BatchRequestVideoMessage : BatchRequestMessage
    {
        public BatchRequestVideoMessage(string openIds, string media_id)
        {
            this.touser = openIds.Split(',').ToList();
            this.mpvideo = new Media() { media_id = media_id };
        }

        public string msgtype { get { return "mpvideo"; } }

        public Media mpvideo { get; set; }
        public class Media
        {
            public string media_id { get; set; }
        }
    }
    /// <summary>
    /// 组群发卡券消息
    /// </summary>
    public class BatchRequestCardMessage : BatchRequestMessage
    {
        public BatchRequestCardMessage(string openIds, string card_id)
        {
            this.touser = openIds.Split(',').ToList();
            this.wxcard = new Card() { card_id = card_id };
        }

        public string msgtype { get { return "wxcard"; } }

        public Card wxcard { get; set; }
        public class Card
        {
            public string card_id { get; set; }
        }
    }
}