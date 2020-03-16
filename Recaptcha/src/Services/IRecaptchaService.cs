using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AlphaKop.Recaptcha.Services {
    public interface IRecaptchaService {
        Task<RecaptchaResponse> Validate(HttpRequest request);
    }
}
