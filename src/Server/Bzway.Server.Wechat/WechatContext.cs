using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Bzway.Common.Utility;
using Bzway.Common.Share;
using Bzway.Module.Wechat.Entity;
using Bzway.Module.Wechat.Model;
using System.Xml;
using Autofac;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bzway.Server.Wechat
{

    public interface IDynamicView<TModel>
    {
        TModel Model { get; }
        ViewDataDictionary<TModel> ViewData { get; set; }
        void WriteLiteral(object obj);
        void Write(object obj);
        void WriteTo(object obj);

        StringBuilder Content { get; }
        Task ExecuteAsync();
    }
    public abstract class DynamicView : IDynamicView<WechatContext>
    {
        public DynamicView()
        {

            this.Content = new StringBuilder();
        }
        public StringBuilder Content { get; protected set; }
        public WechatContext Model { get; protected set; }
        public ViewDataDictionary<WechatContext> ViewData { get; set; }

        public string Layout { get; set; }
        public abstract Task ExecuteAsync();

        public void Write(object obj)
        {
            this.Content.Append(obj);
        }

        public void WriteLiteral(object obj)
        {
            this.Content.Append(obj);
        }

        public void WriteTo(object obj)
        {
            this.Content.Append(obj);
        }
    }
    public class WechatContext
    {
        private readonly HttpContext context;
        private readonly XmlDocument doc;

        public WechatContext(HttpContext context)
        {
            if (context == null)
            {
                this.Signatured = false;
                return;
            }
            this.context = context;
            this.Signatured = this.CheckSignature();
            if (!Signatured)
            {
                return;
            }
            this.doc = new XmlDocument();
            this.doc.Load(context.Request.Body);
        }

        private WechatMessageBase wechatEventMessage;
        public WechatMessageBase Message
        {
            get
            {
                if (wechatEventMessage == null)
                {
                    string msgType = this.doc.GetElementsByTagName("MsgType")[0].InnerText;
                    switch (msgType)
                    {
                        case "text"://文本消息

                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "image"://图片消息
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "location"://地理位置消息
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "link"://链接消息
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "event"://事件推送
                                     //todo foreach event
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "voice"://语音消息
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "video"://视频消息
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        case "shortvideo"://小视频
                            this.wechatEventMessage = SerializationHelper.XmlDeserialize<WechatMessageBase>(this.doc.OuterXml);
                            break;
                        default:
                            break;
                    }
                }
                return this.wechatEventMessage;
            }
        }
        bool CheckSignature()
        {
            string signature = this.context.Request.Query["signature"];
            string echostr = this.context.Request.Query["echostr"];
            string timestamp = this.context.Request.Query["timestamp"];
            string nonce = this.context.Request.Query["nonce"];
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                return false;
            }
            string id = this.context.Request.Path.Value;
            this.CurrentOfficialAccount = CacheManager.Default.MemCacheProvider.Get<WechatOfficialAccount>("Wechat.MessageServer.OfficialAccount." + id, () => { return new WechatOfficialAccount(); });
            if (CurrentOfficialAccount == null)
            {
                return false;
            }
            var token = this.CurrentOfficialAccount.Token;
            var array = new[] { token, timestamp, nonce }.OrderBy(t => t).ToArray();
            var arrayString = string.Join("", array);

            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrayString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            var test = enText.ToString();
            return signature.Equals(enText.ToString());
        }

        public bool Signatured { get; set; }

        public string Echo()
        {
            if ("get".Equals(context.Request.Method, StringComparison.CurrentCultureIgnoreCase))
            {
                return this.context.Request.Query["echostr"];
            }
            //todo insert wechatlog
            return string.Empty;
        }
        public string xml { get; set; }
        public string openId { get; set; }
        public WechatOfficialAccount CurrentOfficialAccount { get; set; }



    }

}