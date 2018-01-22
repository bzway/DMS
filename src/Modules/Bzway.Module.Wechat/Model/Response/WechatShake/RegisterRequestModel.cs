using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class RegisterRequestModel
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string industry_id { get; set; }
        public Array qualification_cert_urls { get; set; }
        public string apply_reason { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AuditStatusResponseModel
    {
        public AuditData data { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public class AuditData
        {
            public string apply_time { get; set; }
            public string audit_comment { get; set; }
            public int audit_status { get; set; }
            public string audit_time { get; set; }
        }
    }
}
