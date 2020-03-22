using AlphaKop.Core.Flows.Middleware;
using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Chains {
    public interface IAsyncTaskChain<TParameter, TReturn> {
        IAsyncTaskChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc);

        IAsyncTaskChain<TParameter, TReturn> Chain<TMiddleware>()
            where TMiddleware : IAsyncMiddleware<TParameter, TReturn>;

        IAsyncTaskChain<TParameter, TReturn> Chain(Type middlewareType);

        Task<TReturn> Execute(TParameter parameter);
    }
}
