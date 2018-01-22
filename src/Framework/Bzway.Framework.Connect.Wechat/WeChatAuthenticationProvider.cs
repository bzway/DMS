using System;
using System.Threading.Tasks;
namespace Microsoft.Owin.Security.Weixin
{
	public class WeChatAuthenticationProvider : IWeChatAuthenticationProvider
	{
		public Func<WeChatAuthenticatedContext, Task> onAuthenticated
		{
			get;
			set;
		}
		public Func<WeChatReturnEndpointContext, Task> onReturnEndpoint
		{
			get;
			set;
		}
		public WeChatAuthenticationProvider()
		{
			this.onAuthenticated = ((WeChatAuthenticatedContext c) => Task.FromResult<WeChatAuthenticatedContext>(null));
			this.onReturnEndpoint = ((WeChatReturnEndpointContext c) => Task.FromResult<WeChatReturnEndpointContext>(null));
		}
		public Task Authenticated(WeChatAuthenticatedContext context)
		{
			return this.onAuthenticated(context);
		}
		public Task ReturnEndpoint(WeChatReturnEndpointContext context)
		{
			return this.onReturnEndpoint(context);
		}
	}
}
