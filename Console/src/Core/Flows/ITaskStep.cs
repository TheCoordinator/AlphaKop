using System.Threading.Tasks;

namespace AlphaKop.Core.Flows {
    public interface ITaskStep<TParameter, TJob> where TJob: struct, IJob {
        TJob? Job { get; set; }
        Task Execute(TParameter parameter);
    }
}