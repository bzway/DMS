﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Module.Wechat.Model
{

    public class WechatCreateTagResponseModel
    {
        public Tag tag { get; set; }

        public class Tag
        {

            public string id { get; set; }
            public string name { get; set; }
        }
    }
}