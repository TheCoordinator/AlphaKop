using AlphaKop.Core.Flows.Middleware;
using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Pipelines {
    public interface IAsyncPipeline<TParameter> {
        IAsyncPipeline<TParameter> Add<TMiddleware>()
            where TMiddleware : IAsyncMiddleware<TParameter>;

        Task Execute(TParameter parameter);

        IAsyncPipeline<TParameter> Add(Type middlewareType);
    }
}
