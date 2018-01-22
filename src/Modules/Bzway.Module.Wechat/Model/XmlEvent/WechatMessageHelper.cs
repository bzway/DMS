using Bzway.Common.Utility;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bzway.Module.Wechat.Model
{
    public class WechatMessageHelper
    {
        private readonly static Dictionary<string, Type> dict = new Dictionary<string, Type>();
        public WechatMessageHelper()
        {
            dict.Add("text", typeof(WechatTextEventMessage));
        }
        public static Type TryGetMessageType(string name)
        {
            if (dict.ContainsKey(name))
            {
                return dict[name];
            }
            return null;
        }

    }
}