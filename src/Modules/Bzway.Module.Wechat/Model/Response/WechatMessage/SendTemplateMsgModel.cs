using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class securityRequest
    {
        public string securityKeyTxt { get; set; }
        public string randomString { get; set; }
        public string signature { get; set; }
    }

    public class templateMsgRequest
    {
        public string openId { get; set; }
        public string templateId { get; set; }
        public string url { get; set; }
        public DateTime sendTime { get; set; }
        public List<parameter> parameter { get; set; }
    }

    public class templateMsgResponse
    {
        public int resultCode { get; set; }
        public string resultMsg { get; set; }
        public string result { get; set; }
        public string randomString { get; set; }
        public string signature { get; set; }
    }

    public class parameter
    {
        public string key { get; set; }
        public string color { get; set; }
        public string value { get; set; }
    }

    public class TemplateMsgModel
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public TemplateMsgData data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class TemplateMsgDataKey
    {
        public string color { get; set; }
        public string value { get; set; }
    }

    public class TemplateMsgData
    {
        public TemplateMsgDataKey first { get; set; }
        public TemplateMsgDataKey keyword1 { get; set; }
        public TemplateMsgDataKey keyword2 { get; set; }
        public TemplateMsgDataKey keyword3 { get; set; }
        public TemplateMsgDataKey remark { get; set; }
    }
}
