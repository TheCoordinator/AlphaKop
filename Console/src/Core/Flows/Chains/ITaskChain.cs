using AlphaKop.Core.Flows.Middleware;
using System;

namespace AlphaKop.Core.Flows.Chains {
    public interface ITaskChain<TParameter, TReturn> {
        ITaskChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFunc);

        ITaskChain<TParameter, TReturn> Chain<TMiddleware>()
            where TMiddleware : IMiddleware<TParameter, TReturn>;

        ITaskChain<TParameter, TReturn> Chain(Type middlewareType);

        TReturn Execute(TParameter parameter);
    }
}
