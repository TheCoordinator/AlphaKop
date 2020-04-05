using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlphaKop.Core.Network.Http {
    public static class ResponseExtensions {
        /// <remarks>
        /// Inspired by http://chocosmith.wordpress.com/2014/05/30/httpresponsemessage-ensuresuccessstatuscode-with-json-result/
        /// </remarks>
        public static async Task EnsureSuccess(this HttpResponseMessage response) {
            if (response.IsSuccessStatusCode)
                return;

            string msg = String.Format("Received HTTP {0} ({1}) while {2}ing URL '{3}'.",
                (int)response.StatusCode,
                response.StatusCode,
                response.RequestMessage.Method,
                response.RequestMessage.RequestUri
            );

            try {
                //read the content if we can
                if (response.Content != null) {
                    string content = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrWhiteSpace(content)) {
                        msg += Environment.NewLine + content;
                    }
                }
            } catch {
                //swallow the exception, we are about to throw a better exception anyway
            }

            throw new HttpRequestException(msg);
        }

        public static IEnumerable<Cookie> GetCookies(this HttpResponseMessage response) {
            response.Headers.TryGetValues("Set-Cookie", out var setCookie);
            
            if (setCookie == null) {
                return Array.Empty<Cookie>();
            }

            return setCookie
                .Select(cookie => cookie.Split(';'))
                .Select(cookies => cookies.FirstOrDefault())
                .Select(cookie => cookie.Split('='))
                .Select(cookieValues => new Cookie(name: cookieValues[0], value: cookieValues[1]));
        }
    }
}