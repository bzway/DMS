﻿using Bzway.Data.Core;

namespace Bzway.Module.Wechat.Entity
{

    public class WechatUserInteraction : EntityBase
    {
        public string OfficialAccount { get; set; }

        /// <summary>
        ///用户的唯一标识
        /// </summary>
        public string OpenId { get; set; }
        public InteractionType Type { get; set; }


        public string MsgId { get; set; }

        public string Content { get; set; }

        public string Remark { get; set; }
    }

    public enum InteractionType
    {
        ReceiveTextMessage, 
        ReceiveScan,
        ReceiveSubscribe, ReceiveScanSubscribe,
        ReceiveImageMessage, cc,
        df, d,
    }
}