using System.Threading.Tasks;

namespace AlphaKop.Core.Flows {
    public interface ITaskStep<TInput> {
        int Retries { get; set; }
        Task Execute(TInput input);
    }

    public interface ITaskStep<TParameter, TJob> where TJob : struct, IJob {
        TJob? Job { get; set; }
        int Retries { get; set; }
        Task Execute(TParameter parameter);
    }
}