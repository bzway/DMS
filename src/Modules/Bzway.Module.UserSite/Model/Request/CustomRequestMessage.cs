
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{
    /// <summary>
    /// 发送客服消息
    /// </summary>
    public class CustomRequestMessage
    {
        public string touser { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    /// <summary>
    /// 发送文本消息
    /// </summary>
    public class TextMessageRequest : CustomRequestMessage
    {
        public TextMessageRequest(string touser, string content)
        {
            this.touser = touser;
            this.text = new Text() { content = content };
        }
        public string msgtype { get { return "text"; } }

        public Text text { get; set; }

        public class Text
        {
            public string content { get; set; }
        }
    }
    /// <summary>
    /// 发送图片消息
    /// </summary>
    public class ImageMessageRequest : CustomRequestMessage
    {
        public ImageMessageRequest(string touser, string media_id)
        {
            this.touser = touser;
            this.image = new Image() { media_id = media_id };
        }

        public string msgtype { get { return "image"; } }
        public Image image { get; set; }

        public class Image
        {
            public string media_id { get; set; }
        }
    }

    /// <summary>
    /// 发送语音消息
    /// </summary>
    public class VoiceMessageRequest : CustomRequestMessage
    {
        public VoiceMessageRequest(string touser, string media_id)
        {
            this.touser = touser;
            this.voice = new Voice() { media_id = media_id };
        }
        public string msgtype { get { return "voice"; } }
        public Voice voice { get; set; }

        public class Voice
        {
            public string media_id { get; set; }
        }
    }

    /// <summary>
    /// 发送视频消息
    /// </summary>
    public class VideoMessageRequest : CustomRequestMessage
    {
        public VideoMessageRequest(string touser, string media_id, string title, string description)
        {
            this.touser = touser;
            this.video = new Video()
            {
                media_id = media_id,
                title = title,
                description = description,
            };
        }
        public string msgtype { get { return "video"; } }

        public Video video { get; set; }

        public class Video
        {
            public string media_id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
        }
    }
    /// <summary>
    /// 发送音乐消息
    /// </summary>
    public class MusicMessageRequest : CustomRequestMessage
    {
        public MusicMessageRequest(string touser, string title, string description, string hqmusicurl, string musicurl, string thumb_media_id)
        {
            this.touser = touser;
            this.music = new Articles()
            {
                thumb_media_id = thumb_media_id,
                title = title,
                description = description,
                hqmusicurl = hqmusicurl,
                musicurl = musicurl,
            };
        }
        public string msgtype { get { return "music"; } }

        public Articles music { get; set; }

        public class Articles
        {
            public string title { get; set; }
            public string description { get; set; }
            public string musicurl { get; set; }
            public string hqmusicurl { get; set; }
            public string thumb_media_id { get; set; }
        }
    }
    /// <summary>
    /// 发送图文消息
    /// </summary>
    public class NewsMessageRequest : CustomRequestMessage
    {
        public NewsMessageRequest(string touser)
        {
            this.touser = touser;
            this.news = new WechatNews { articles = new List<Articles>() };
        }
        public string msgtype { get { return "news"; } }

        public void AddArticles(string title, string description, string url, string piccurl)
        {
            this.news.articles.Add(new Articles() { description = description, title = title, piccurl = piccurl, url = url });
        }
        public WechatNews news { get; set; }

        public class WechatNews
        {
            public List<Articles> articles { get; set; }
        }
        public class Articles
        {
            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string piccurl { get; set; }
        }
    }


}