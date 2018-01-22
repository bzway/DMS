using System;
using System.Net.Http;
namespace Microsoft.Owin.Security.Weixin
{
    public class WeChatAuthenticationMiddleware : AuthenticationMiddleware<WeChatAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        public WeChatAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, WeChatAuthenticationOptions options) : base(next, options)
        {
            this._logger = app.CreateLogger<WeChatAuthenticationOptions>();
            if (base.Options.Provider == null)
            {
                base.Options.Provider = new WeChatAuthenticationProvider();
            }
            if (base.Options.StateDataFormat == null)
            {
                IDataProtector protector = app.CreateDataProtector(new string[]
                {
                    typeof(WeChatAuthenticationMiddleware).FullName,
                    base.Options.AuthenticationType,
                    "v1"
                });
                base.Options.StateDataFormat = new PropertiesDataFormat(protector);
            }
            this._httpClient = new HttpClient(WeChatAuthenticationMiddleware.ResolveHttpMessageHandler(base.Options));
            this._httpClient.Timeout = base.Options.BackchannelTimeout;
            this._httpClient.MaxResponseContentBufferSize = 10485760L;
        }
        protected override AuthenticationHandler<WeChatAuthenticationOptions> CreateHandler()
        {
            return new WeChatAccountAuthenticationHandler(this._httpClient, this._logger);
        }
        private static HttpMessageHandler ResolveHttpMessageHandler(WeChatAuthenticationOptions options)
        {
            HttpMessageHandler httpMessageHandler = options.BackchannelHttpHandler ?? new WebRequestHandler();
            if (options.BackchannelCertificateValidator != null)
            {
                WebRequestHandler webRequestHandler = httpMessageHandler as WebRequestHandler;
                if (webRequestHandler == null)
                {
                    throw new InvalidOperationException("An ICertificateValidator cannot be specified at the same time as an HttpMessageHandler unless it is a WebRequestHandler.");
                }
                webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(options.BackchannelCertificateValidator.Validate);
            }
            return httpMessageHandler;
        }
    }
}
