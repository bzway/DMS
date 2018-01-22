using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Microsoft.Owin.Security.Weixin
{
    internal class WeChatAccountAuthenticationHandler : AuthenticationHandler<WeChatAuthenticationOptions>
	{
	 
		public class AccessTokenResult
		{
			public string errcode
			{
				get;
				set;
			}
			public string errmsg
			{
				get;
				set;
			}
			public string access_token
			{
				get;
				set;
			}
			public string expires_in
			{
				get;
				set;
			}
			public string refresh_token
			{
				get;
				set;
			}
			public string openid
			{
				get;
				set;
			}
			public string scope
			{
				get;
				set;
			}
		}
		private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
		private const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/qrconnect";
		private const string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";
		private const string UserInfoEndpoint = "https://api.weixin.qq.com/sns/userinfo";
		private const string OpenIDEndpoint = "https://api.weixin.qq.com/sns/oauth2";
		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;
		public WeChatAccountAuthenticationHandler(HttpClient httpClient, ILogger logger)
		{
			this._httpClient = httpClient;
			this._logger = logger;
		}

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AccessTokenResult result = new AccessTokenResult() {  };
        }

        //private async Task<bool> InvokeReturnPathAsync()
        //{

        //	AuthenticationTicket authenticationTicket = await base.AuthenticateAsync();
        //	WeChatReturnEndpointContext weChatReturnEndpointContext = new WeChatReturnEndpointContext(base.Context, authenticationTicket);
        //	weChatReturnEndpointContext.SignInAsAuthenticationType = base.Options.SignInAsAuthenticationType;
        //	weChatReturnEndpointContext.RedirectUri = authenticationTicket.Properties.RedirectUri;
        //	authenticationTicket.Properties.RedirectUri = null;
        //	await base.Options.Provider.ReturnEndpoint(weChatReturnEndpointContext);
        //	if (weChatReturnEndpointContext.SignInAsAuthenticationType != null && weChatReturnEndpointContext.Identity != null)
        //	{
        //		ClaimsIdentity claimsIdentity = weChatReturnEndpointContext.Identity;
        //		if (!string.Equals(claimsIdentity.AuthenticationType, weChatReturnEndpointContext.SignInAsAuthenticationType, StringComparison.Ordinal))
        //		{
        //			claimsIdentity = new ClaimsIdentity(claimsIdentity.Claims, weChatReturnEndpointContext.SignInAsAuthenticationType, claimsIdentity.NameClaimType, claimsIdentity.RoleClaimType);
        //		}
        //		base.Context.Authentication.SignIn(weChatReturnEndpointContext.Properties, new ClaimsIdentity[]
        //		{
        //			claimsIdentity
        //		});
        //	}
        //	if (!weChatReturnEndpointContext.IsRequestCompleted && weChatReturnEndpointContext.RedirectUri != null)
        //	{
        //		base.Response.Redirect(weChatReturnEndpointContext.RedirectUri);
        //		weChatReturnEndpointContext.RequestCompleted();
        //	}
        //	return weChatReturnEndpointContext.IsRequestCompleted;
        //}
        //protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        //{
        //	this._logger.WriteVerbose("AuthenticateCore");
        //	AuthenticationProperties authenticationProperties = null;
        //	AuthenticationTicket result;
        //	try
        //	{
        //		string value = null;
        //		string protectedText = null;
        //		IReadableStringCollection query = base.Request.Query;
        //		IList<string> values = query.GetValues("code");
        //		if (values != null && values.Count == 1)
        //		{
        //			value = values[0];
        //		}
        //		values = query.GetValues("state");
        //		if (values != null && values.Count == 1)
        //		{
        //			protectedText = values[0];
        //		}
        //		authenticationProperties = base.Options.StateDataFormat.Unprotect(protectedText);
        //		if (authenticationProperties == null)
        //		{
        //			result = null;
        //			return result;
        //		}
        //		if (!base.ValidateCorrelationId(authenticationProperties, this._logger))
        //		{
        //			result = new AuthenticationTicket(null, authenticationProperties);
        //			return result;
        //		}
        //		List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>
        //		{
        //			new KeyValuePair<string, string>("appid", base.Options.AppId),
        //			new KeyValuePair<string, string>("secret", base.Options.AppSecret),
        //			new KeyValuePair<string, string>("code", value),
        //			new KeyValuePair<string, string>("grant_type", "authorization_code")
        //		};
        //		FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
        //		HttpResponseMessage httpResponseMessage = await this._httpClient.PostAsync("https://api.weixin.qq.com/sns/oauth2/access_token", content, base.Request.CallCancelled);
        //		httpResponseMessage.EnsureSuccessStatusCode();
        //		string s = await httpResponseMessage.Content.ReadAsStringAsync();
        //		JsonSerializer jsonSerializer = new JsonSerializer();
        //		WeChatAccountAuthenticationHandler.AccessTokenResult accessTokenResult = jsonSerializer.Deserialize<WeChatAccountAuthenticationHandler.AccessTokenResult>(new JsonTextReader(new StringReader(s)));
        //		if (accessTokenResult == null || accessTokenResult.access_token == null)
        //		{
        //			this._logger.WriteWarning("Access token was not found", new string[0]);
        //			result = new AuthenticationTicket(null, authenticationProperties);
        //			return result;
        //		}
        //		string requestUri = "https://api.weixin.qq.com/sns/userinfo?access_token=" + Uri.EscapeDataString(accessTokenResult.access_token) + "&openid=" + Uri.EscapeDataString(accessTokenResult.openid);
        //		HttpResponseMessage httpResponseMessage2 = await this._httpClient.GetAsync(requestUri, base.Request.CallCancelled);
        //		httpResponseMessage2.EnsureSuccessStatusCode();
        //		string json = await httpResponseMessage2.Content.ReadAsStringAsync();
        //		JObject user = JObject.Parse(json);
        //		WeChatAuthenticatedContext weChatAuthenticatedContext = new WeChatAuthenticatedContext(base.Context, accessTokenResult.openid, user, accessTokenResult.access_token);
        //		weChatAuthenticatedContext.Identity = new ClaimsIdentity(new Claim[]
        //		{
        //			new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", weChatAuthenticatedContext.Id, "http://www.w3.org/2001/XMLSchema#string", base.Options.AuthenticationType),
        //			new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", weChatAuthenticatedContext.Name, "http://www.w3.org/2001/XMLSchema#string", base.Options.AuthenticationType),
        //			new Claim("urn:wechatconnect:id", weChatAuthenticatedContext.Id, "http://www.w3.org/2001/XMLSchema#string", base.Options.AuthenticationType),
        //			new Claim("urn:wechatconnect:name", weChatAuthenticatedContext.Name, "http://www.w3.org/2001/XMLSchema#string", base.Options.AuthenticationType)
        //		});
        //		await base.Options.Provider.Authenticated(weChatAuthenticatedContext);
        //		weChatAuthenticatedContext.Properties = authenticationProperties;
        //		result = new AuthenticationTicket(weChatAuthenticatedContext.Identity, weChatAuthenticatedContext.Properties);
        //		return result;
        //	}
        //	catch (Exception ex)
        //	{
        //		this._logger.WriteError(ex.Message);
        //	}
        //	result = new AuthenticationTicket(null, authenticationProperties);
        //	return result;
        //}
        //protected override Task ApplyResponseChallengeAsync()
        //{
        //	this._logger.WriteVerbose("ApplyResponseChallenge");
        //	Task result;
        //	if (base.Response.StatusCode != 401)
        //	{
        //		result = Task.FromResult<object>(null);
        //	}
        //	else
        //	{
        //		AuthenticationResponseChallenge authenticationResponseChallenge = base.Helper.LookupChallenge(base.Options.AuthenticationType, base.Options.AuthenticationMode);
        //		if (authenticationResponseChallenge != null)
        //		{
        //			string text = base.Request.Scheme + "://" + base.Request.Host;
        //			string value = base.Request.QueryString.Value;
        //			string redirectUri = string.IsNullOrEmpty(value) ? (text + base.Request.PathBase + base.Request.Path) : string.Concat(new object[]
        //			{
        //				text,
        //				base.Request.PathBase,
        //				base.Request.Path,
        //				"?",
        //				value
        //			});
        //			string stringToEscape = text + base.Request.PathBase + base.Options.ReturnEndpointPath;
        //			AuthenticationProperties properties = authenticationResponseChallenge.Properties;
        //			if (string.IsNullOrEmpty(properties.RedirectUri))
        //			{
        //				properties.RedirectUri = redirectUri;
        //			}
        //			base.GenerateCorrelationId(properties);
        //			string stringToEscape2 = string.Join(",", base.Options.Scope);
        //			string stringToEscape3 = base.Options.StateDataFormat.Protect(properties);
        //			string location = string.Concat(new string[]
        //			{
        //				"https://open.weixin.qq.com/connect/qrconnect?appid=",
        //				Uri.EscapeDataString(base.Options.AppId ?? string.Empty),
        //				"&redirect_uri=",
        //				Uri.EscapeDataString(stringToEscape),
        //				"&scope=",
        //				Uri.EscapeDataString(stringToEscape2),
        //				"&state=",
        //				Uri.EscapeDataString(stringToEscape3),
        //				"&response_type=code"
        //			});
        //			base.Response.Redirect(location);
        //		}
        //		result = Task.FromResult<object>(null);
        //	}
        //	return result;
        //}
        //private string GenerateRedirectUri()
        //{
        //	string arg = base.Request.Scheme + "://" + base.Request.Host;
        //	return arg + base.RequestPathBase + base.Options.ReturnEndpointPath;
        //}

        //      protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //      {
        //          bool result;
        //          if (base.Options.ReturnEndpointPath != null && string.Equals(base.Options.ReturnEndpointPath, base.Request.Path.Value, StringComparison.OrdinalIgnoreCase))
        //          {
        //              result = await this.InvokeReturnPathAsync();
        //          }
        //          else
        //          {
        //              result = false;
        //          }
        //          return result;
        //      }
    }
}
