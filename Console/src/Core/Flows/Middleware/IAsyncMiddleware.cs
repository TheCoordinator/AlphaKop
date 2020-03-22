using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Middleware {
    public interface IAsyncMiddleware<TParameter> {
        Task Run(TParameter parameter, Func<TParameter, Task> next);
    }
}
