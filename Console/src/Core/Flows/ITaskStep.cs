using System.Threading.Tasks;

namespace AlphaKop.Core.Flows {
    public interface ITaskStep<TParameter, TJob> where TJob: struct {
        TJob? Job { get; set; }
        Task Execute(TParameter parameter);
    }
}