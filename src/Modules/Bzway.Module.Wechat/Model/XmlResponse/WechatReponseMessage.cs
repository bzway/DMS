using Bzway.Common.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bzway.Module.Wechat.Model
{

    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatText : WechatReponseModel
    {
        /// <summary>
        ///回复的消息内容（换行:在content中能够换行，微信客户端就支持换行显示）	
        /// </summary>
        public string Content { get; set; }
        public WechatText(string FromUserName, string ToUserName, string Content)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.Content = Content;
            this.MsgType = "text";
        }
    }





    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatImage : WechatReponseModel
    {

        public WechatImage(string FromUserName, string ToUserName, string MediaId)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.MsgType = "image";
            this.Image = new ImageModel() { MediaId = MediaId };
        }


        public ImageModel Image { get; set; }

        public class ImageModel
        {
            /// <summary>
            /// 通过素材管理接口上传多媒体文件，得到的id。
            /// </summary>
            public string MediaId { get; set; }
        }
    }
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatVoice : WechatReponseModel
    {

        public WechatVoice(string FromUserName, string ToUserName, string MediaId)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.MsgType = "voice";
            this.Voice = new VoiceModel() { MediaId = MediaId };
        }


        public VoiceModel Voice { get; set; }
        public class VoiceModel
        {
            /// <summary>
            /// 通过素材管理接口上传多媒体文件，得到的id。
            /// </summary>
            public string MediaId { get; set; }
        }
    }
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatVideo : WechatReponseModel
    {

        public WechatVideo(string FromUserName, string ToUserName, string MediaId, string Title, string Description)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.MsgType = "video";
            this.Video = new WechatVideo.VideoModel() { MediaId = MediaId, Description = "", Title = "" };
        }



        public VideoModel Video { get; set; }
        public class VideoModel
        {
            /// <summary>
            /// 通过素材管理接口上传多媒体文件，得到的id。
            /// </summary>
            public string MediaId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

    }
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatMusic : WechatReponseModel
    {

        public WechatMusic(string FromUserName, string ToUserName, string MediaId, string Title, string Description, string MusicUrl, string HQMusicUrl)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.MsgType = "music";
            this.Music = new MusicModel()
            {
                ThumbMediaId = MediaId,
                HQMusicUrl = HQMusicUrl,
                MusicUrl = MusicUrl,
                Description = Description,
                Title = Title
            };
        }

        public MusicModel Music { get; set; }
        public class MusicModel
        {

            public string Title { get; set; }
            public string Description { get; set; }
            public string MusicUrl { get; set; }
            public string HQMusicUrl { get; set; }
            public string ThumbMediaId { get; set; }
        }
    }
    [XmlRoot(ElementName = "xml", Namespace = null)]
    public class WechatArticle : WechatReponseModel
    {

        public WechatArticle(string FromUserName, string ToUserName, string Title, string Description, string Url, string PicUrl)
        {
            this.FromUserName = ToUserName;
            this.ToUserName = FromUserName;
            this.MsgType = "news";
            this.Articles = new List<ArticleModel>(8);
            this.Articles.Add(new ArticleModel()
            {
                PicUrl = PicUrl,
                Url = Url,
                Description = Description,
                Title = Title
            });
        }
        public void AddArticle(string Title, string Description, string Url, string PicUrl)
        {
            this.Articles.Add(new ArticleModel()
            {
                PicUrl = PicUrl,
                Url = Url,
                Description = Description,
                Title = Title
            });
        }
      
        public int ArticleCount { get { return this.Articles.Count; } set { } }
        [XmlArrayItem(ElementName = "item")]
        public List<ArticleModel> Articles { get; set; }

        public class ArticleModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string PicUrl { get; set; }
            public string Url { get; set; }
        }
    }



    public class CustomerMessage
    {
        public string touser { get; set; }

        public string msgtype { get; set; }

        public news news { get; set; }
    }
}