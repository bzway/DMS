using Bzway.Data.Core;
using System;

namespace Bzway.Module.Wechat.Entity
{

    public class WechatQRCode : EntityBase
    {
        public string OfficialAccount { get; set; }

        /// <summary>
        ///获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        ///该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。
        /// </summary>
        public DateTime ExpiredTime { get; set; }
        /// <summary>
        /// 已经生成好的二维码资源链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 场景
        /// </summary>
        public string Scene { get; set; }
        /// <summary>
        /// QR Code 中的内容
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否已经使用
        /// </summary>
        public bool IsUsed { get; set; }

        public QRCodeType Type { get; set; }
        /// <summary>
        /// 占用者的唯一编号
        /// </summary>
        public string OwnerID { get; set; }
    }

    public enum QRCodeType
    {
        None = 0,
        LimitScene = 1,
        TemporalScene = 2,
        ScanPointScene = 3,

    }
}