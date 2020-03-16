using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaKop.Recaptcha.Models {
    public sealed class V2Model {
        public string Id { get; set; }
        public Guid Uid { get; set; }
        public string Method { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
        public RecaptchaSettings Settings { get; set; }
        public string? SuccessCallback { get; set; }
        public string? ErrorCallback { get; set; }
        public string? ExpiredCallback { get; set; }

        public V2Model(
            string id,
            Guid uid,
            string method,
            string theme,
            string language,
            RecaptchaSettings settings,
            string? successCallback,
            string? errorCallback,
            string? expiredCallback
        ) {
            Id = id;
            Uid = uid;
            Method = method;
            Theme = theme;
            Language = language;
            Settings = settings;
            SuccessCallback = successCallback;
            ErrorCallback = errorCallback;
            ExpiredCallback = expiredCallback;
        }
    }
}
