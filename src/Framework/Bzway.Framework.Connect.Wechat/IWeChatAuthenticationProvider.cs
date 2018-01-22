using System.Threading.Tasks;
namespace Microsoft.Owin.Security.Weixin
{
    public interface IWeChatAuthenticationProvider
	{
		Task Authenticated(WeChatAuthenticatedContext context);
		Task ReturnEndpoint(WeChatReturnEndpointContext context);
	}
}
