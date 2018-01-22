namespace Microsoft.Owin.Security.Weixin
{
    public class WeChatReturnEndpointContext : ReturnEndpointContext
	{
		public WeChatReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket) : base(context, ticket)
		{
		}
	}
}
