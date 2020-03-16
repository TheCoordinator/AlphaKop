using System;
using System.Numerics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaKop.Recaptcha.Models;
using AlphaKop.Recaptcha.Templates;

namespace AlphaKop.Recaptcha.Extensions {
    public static class IHtmlHelperRecaptchaExtensions {
        /// <summary>
        /// Helper extension to render the Google Recaptcha.
        /// </summary>
        /// <param name="helper">Html helper object.</param>
        /// <param name="settings">Recaptcha settings needed to render.</param>
        /// <param name="theme">Google Recaptcha theme default is light</param>
        /// <param name="action">Google Recaptcha v3 <a href="https://developers.google.com/recaptcha/docs/v3#actions">Action</a></param>
        /// <param name="language">Google Recaptcha <a href="https://developers.google.com/recaptcha/docs/language">Language Code</a></param>
        /// <param name="id">Google Recaptcha v2-invis button id. This id can't be named submit due to a naming bug.</param>
        /// <param name="successCallback">Google Recaptcha v2/v2-invis success callback method.</param>
        /// <param name="errorCallback">Google Recaptcha v2/v2-invis error callback method.</param>
        /// <param name="expiredCallback">Google Recaptcha v2/v2-invis expired callback method.</param>
        /// <returns>HtmlString with Recaptcha elements</returns>
        public static HtmlString Recaptcha(
            this IHtmlHelper helper,
            RecaptchaSettings settings,
            string theme = "light",
            string action = "homepage",
            string language = "en",
            string id = "recaptcha",
            string? successCallback = null,
            string? errorCallback = null,
            string? expiredCallback = null
        ) {
            if (!settings.Enabled) return new HtmlString("<!-- Google Recaptcha disabled -->");

            var uid = Guid.NewGuid();
            var method = uid.ToString().Replace("-", "_");

            var v2Model = new V2Model(id: id,
                                      uid: uid,
                                      method: method,
                                      theme: theme,
                                      language: language,
                                      settings: settings,
                                      successCallback: successCallback,
                                      errorCallback: errorCallback,
                                      expiredCallback: expiredCallback);
            var v2 = new V2(v2Model);

            return new HtmlString(v2.TransformText());
        }
    }
}
