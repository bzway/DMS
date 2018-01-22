using System.Collections.Generic;

namespace Bzway.Framework.Core.Wechat.Models
{
    /// <summary>
    /// GetMenu返回的Json结果
    /// </summary>
    internal class WechatGetWechatMenuJsonResultModel : WechatJsonResultModel
    {
        public ButtonGroup menu { get; set; }


        public class ButtonGroup
        {
            public List<Button> button { get; set; }
        }

        public class Button
        {
            public string type { get; set; }
            public string name { get; set; }
            public string key { get; set; }
            public string url { get; set; }
            public string media_id { get; set; }
            public List<Button> sub_button { get; set; }
        }
    }
}
