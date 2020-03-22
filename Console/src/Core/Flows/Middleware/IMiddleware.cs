using System;

namespace AlphaKop.Core.Flows.Middleware {
    public interface IMiddleware<TParameter> {
        void Run(TParameter parameter, Action<TParameter> next);
    }
}
