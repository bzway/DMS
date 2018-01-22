using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Bzway.Module.Wechat.Model
{
    public class ResultCode
    {
        public const string Success = "0";
        public const string AppIdIsInvalid = "1";
        public const string StateIsInvalid = "2";
        public const string AuthError = "3";
        public const string AccountIsInvalid = "4";
        public const string AccessTokenMissed = "5";
        public const string FunctionMissed = "6";
        public const string SystemError = "-100";
    }
}