
using Newtonsoft.Json;

namespace Bzway.Framework.Core.Wechat.Models
{
    public class GroupBatchRequestMessage
    {
        public Filter filter { get; set; }
        public class Filter
        {
            public bool is_to_all { get; set; }
            public string group_id { get; set; }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    /// <summary>
    /// 组群发图文消息
    /// </summary>
    public class GroupBatchRequestNewsMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestNewsMessage(string group_id, string media_id)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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
    public class GroupBatchRequestTextMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestTextMessage(string group_id, string content)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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
    public class GroupBatchRequestVoiceMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestVoiceMessage(string group_id, string media_id)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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

    public class GroupBatchRequestImageMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestImageMessage(string group_id, string media_id)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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
    public class GroupBatchRequestVideoMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestVideoMessage(string group_id, string media_id)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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
    public class GroupBatchRequestCardMessage : GroupBatchRequestMessage
    {
        public GroupBatchRequestCardMessage(string group_id, string card_id)
        {
            this.filter = new Filter() { is_to_all = false, group_id = group_id };
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