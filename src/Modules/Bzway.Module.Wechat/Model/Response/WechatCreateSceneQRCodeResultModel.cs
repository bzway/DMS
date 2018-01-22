namespace Bzway.Module.Wechat
{
    /// <summary>
    /// access_token请求后的JSON返回格式
    /// </summary>
    public class WechatCreateSceneQRCodeResultModel : WechatBaseResponseModel
    {
        /// <summary>
        ///获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        ///该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。
        /// </summary>
        public string expire_seconds { get; set; }
        /// <summary>
        ///二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        public string url { get; set; }

    }
}
