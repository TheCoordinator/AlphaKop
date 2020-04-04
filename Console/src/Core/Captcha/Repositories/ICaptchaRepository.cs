using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;

namespace AlphaKop.Core.Captcha.Repositories {
    public interface ICaptchaRepository {
        Task<CaptchaResponse> FetchCaptcha();
        Task TriggerCaptcha(CaptchaRequest request);
        Task CancelTriggerCaptcha(CaptchaRequest request);
    }
}
