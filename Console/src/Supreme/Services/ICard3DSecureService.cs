using System.Threading.Tasks;

namespace AlphaKop.Supreme.Services {
    public interface ICard3DSecureService {
        Task<Card3DSecureResponse> FetchCard3DSecure(string htmlContent);
    }
}
