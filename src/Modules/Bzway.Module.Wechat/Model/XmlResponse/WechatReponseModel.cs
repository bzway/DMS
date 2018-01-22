using Bzway.Common.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bzway.Module.Wechat.Model
{
    public class WechatReponseModel
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）	  
        /// </summary>
        public virtual string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号	
        /// </summary>
        public virtual string FromUserName { get; set; }

        ///<summary>
        ///消息创建时间 （整型）
        /// </summary>
        public virtual string CreateTime { get; set; }
        public string MsgType { get; set; }

        public override string ToString()
        {
            this.CreateTime = DateTimeHelper.GetBaseTimeValue(DateTime.Now).ToString();

            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            return FormartResult(result);
        }
        private string FormartResult(string result)
        {
            result = result.Replace("<?xml version=\"1.0\"?>", "");
            var count = result.Split('>')[0].Count() + 1;
            result = "<xml>" + result.Remove(0, count);
            return result;
        }

    }
}