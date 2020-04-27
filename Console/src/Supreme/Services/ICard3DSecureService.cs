using System.Threading.Tasks;

namespace AlphaKop.Supreme.Services {
    public interface ICard3DSecureService {
        Task<string> FetchCardinalId(string htmlContent);
    }
}
