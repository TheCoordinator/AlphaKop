using System.Threading.Tasks;
using AlphaKop.Core.Captcha.Network;

namespace AlphaKop.Core.Captcha.Repositories {
    public interface ICaptchaRepository {
        Task<CaptchaResponse> fetchCaptcha();
        Task triggerCaptcha(CaptchaRequest request);
        Task cancelTriggerCaptcha(CaptchaRequest request);
    }
}
