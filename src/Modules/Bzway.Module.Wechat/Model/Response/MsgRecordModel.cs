using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Module.Wechat.Model
{
    public class MsgRecordModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public int retcode { get; set; }
        public List<recordlist> recordlist { get; set; }

    }

    public class recordlist
    {
        public string openid { get; set; }
        public int opercode { get; set; }
        public string text { get; set; }
        public long time { get; set; }
        public string worker { get; set; }
    }

    public class MsgRecordPostModel
    {
        public int endtime { get; set; }
        public int pageindex { get; set; }
        public int pagesize { get; set; }
        public int starttime { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
