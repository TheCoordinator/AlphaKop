using AlphaKop.Core.Flows.Middleware;
using System;

namespace AlphaKop.Core.Flows.Pipelines {
    public interface IPipeline<TParameter> {
        IPipeline<TParameter> Add<TMiddleware>()
            where TMiddleware : IMiddleware<TParameter>;

        void Execute(TParameter parameter);

        IPipeline<TParameter> Add(Type middlewareType);
    }
}
