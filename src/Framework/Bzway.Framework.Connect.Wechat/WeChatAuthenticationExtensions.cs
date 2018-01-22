using Microsoft.Owin.Security.Weixin;
using System;
namespace Owin
{
    public static class WeChatAuthenticationExtensions
	{
		public static void UseWeChatAuthentication(this IAppBuilder app, WeChatAuthenticationOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			app.Use(typeof(WeChatAuthenticationMiddleware), new object[]
			{
				app,
				options
			});
		}
		public static void UseWeChatAuthentication(this IAppBuilder app, string appId, string appSecret)
		{
			app.UseWeChatAuthentication(new WeChatAuthenticationOptions
			{
				AppId = appId,
				AppSecret = appSecret,
				SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType()
			});
		}
	}
}
