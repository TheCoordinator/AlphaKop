using AlphaKop.Core.Captcha.Network;
using System.Threading.Tasks;

namespace AlphaKop.Core.Captcha.Repositories {
    public interface ICaptchaRepository {
        Task<CaptchaResponse> FetchCaptcha();
        Task TriggerCaptcha(CaptchaRequest request);
        Task CancelTriggerCaptcha(CaptchaRequest request);
    }
}
