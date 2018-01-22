using System;
using Microsoft.Extensions.Options;
using Bzway.Framework.Connect.Authentication;
using Microsoft.AspNetCore.Builder;

namespace Bzway.Framework.Connect.Authentication
{
    /// <summary>
    /// Extension methods to add cookie authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class CookieAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="CookieAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables cookie authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBzwayCookieAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CookieAuthenticationMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="CookieAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables cookie authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="CookieAuthenticationOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder app, CookieAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<CookieAuthenticationMiddleware>(Options.Create(options));
        }
    }
}