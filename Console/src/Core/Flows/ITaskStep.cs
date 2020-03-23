using System.Threading.Tasks;

namespace AlphaKop.Core.Flows {
    public interface ITaskStep<TParameter> {
        Task Execute(TParameter parameter);
    }
}