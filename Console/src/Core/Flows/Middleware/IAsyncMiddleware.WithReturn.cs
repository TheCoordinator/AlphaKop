using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Middleware {
    public interface IAsyncMiddleware<TParameter, TReturn> {
        Task<TReturn> Run(TParameter parameter, Func<TParameter, Task<TReturn>> next);
    }
}
