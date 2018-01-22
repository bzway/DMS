
using Newtonsoft.Json;

namespace Bzway.Framework.Core.Wechat.Models
{
    /// <summary>
    /// 发送模板消息
    /// </summary>
    public class TemplateMessageRequest
    {
        public string touser { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
        public string template_id { get; set; }
        public string url { get; set; }
        public object data { get; set; }
    }
}