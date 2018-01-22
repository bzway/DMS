using Bzway.Data.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bzway.Module.Wechat.Entity
{
    public class WechatLog : EntityBase
    {
        public string OfficialAccount { get; set; }
        [StringLength(100)]
        public string From { get; set; }
        [StringLength(100)]
        public string To { get; set; }
        public string Content { get; set; }
        public EventType Type { get; set; }
    }
    public enum EventType
    {

        UnSubscribe = 0,
        Subscribe = 1,
        Click = 2,
        View = 3,
        Scan = 4,

        //1 文本消息
        ReceiveText = 10,
        //2 图片消息
        ReceiveImage = 11,
        //3 语音消息
        ReceiveVoice = 12,
        //4 视频消息
        ReceiveVideo = 13,
        //5 小视频消息
        ReceiveShortVideo = 14,
        //6 地理位置消息
        ReceiveLocation = 15,
        //7 链接消息
        ReceiveLink = 16,


        /// <summary>
        /// 发送文本消息
        /// </summary>
        SendText = 20,
        /// <summary>
        /// 发送图片消息
        /// </summary>
        SendImage = 21,
        /// <summary>
        /// 发送语音消息
        /// </summary>
        SendVoice = 22,
        /// <summary>
        /// 发送视频消息
        /// </summary>
        SendVideo = 23,

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        SendMusic = 24,
        /// <summary>
        /// 发送图文消息
        /// </summary>
        SendNews = 25,
        /// <summary>
        /// 发送卡券
        /// </summary>
        SendCard = 26,
    }
}