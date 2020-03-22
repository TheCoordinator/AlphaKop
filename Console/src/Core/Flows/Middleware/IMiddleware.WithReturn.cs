using System;

namespace AlphaKop.Core.Flows.Middleware {
    public interface IMiddleware<TParameter, TReturn> {
        TReturn Run(TParameter parameter, Func<TParameter, TReturn> next);
    }
}
