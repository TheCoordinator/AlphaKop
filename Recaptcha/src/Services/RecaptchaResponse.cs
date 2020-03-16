using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaKop.Recaptcha.Services {
    public sealed class RecaptchaResponse {
        public bool success { get; set; }
        public decimal score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }

        internal RecaptchaResponse(
            bool success,
            decimal score,
            string action,
            DateTime challenge_ts,
            string hostname
        ) {
            this.success = success;
            this.score = score;
            this.action = action;
            this.challenge_ts = challenge_ts;
            this.hostname = hostname;
        }
    }
}
