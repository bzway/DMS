using System;
using System.Collections.Generic;
namespace Microsoft.Owin.Security.Weixin
{
    public class WeChatAuthenticationOptions : AspNetCore.Builder.AuthenticationOptions
    {
		public const string AUTHENTICATION_TYPE = "WeChat";
		public ISecureDataFormat<AuthenticationProperties> StateDataFormat
		{
			get;
			set;
		}
		public TimeSpan BackchannelTimeout
		{
			get;
			set;
		}
		public WebRequestHandler BackchannelHttpHandler
		{
			get;
			set;
		}
		public IWeChatAuthenticationProvider Provider
		{
			get;
			set;
		}
		public ICertificateValidator BackchannelCertificateValidator
		{
			get;
			set;
		}
		public IList<string> Scope
		{
			get;
			private set;
		}
		public string ReturnEndpointPath
		{
			get;
			set;
		}
		public string SignInAsAuthenticationType
		{
			get;
			set;
		}
		public string Caption
		{
			get
			{
				return base.Description.Caption;
			}
			set
			{
				base.Description.Caption = value;
			}
		}
		public string AppId
		{
			get;
			set;
		}
		public string AppSecret
		{
			get;
			set;
		}
		public WeChatAuthenticationOptions() : base("WeChat")
		{
			this.Caption = "微信账号";
			this.ReturnEndpointPath = "/signin-wechatconnect";
			base.AuthenticationMode = AuthenticationMode.Passive;
			this.Scope = new List<string>
			{
				"get_user_info"
			};
			this.BackchannelTimeout = TimeSpan.FromSeconds(60.0);
		}
	}
}
