using System.Threading.Tasks;

namespace AlphaKop.Core.Flows {
    public interface ITaskStep<TInput> {
        int Retries { get; set; }
        Task Execute(TInput input);
    }
}