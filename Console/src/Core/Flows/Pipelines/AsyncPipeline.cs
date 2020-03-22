using AlphaKop.Core.Flows.Middleware;
using AlphaKop.Core.Flows.MiddlewareResolver;
using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Pipelines {
    public sealed class AsyncPipeline<TParameter> : BaseMiddlewareFlow<IAsyncMiddleware<TParameter>>, IAsyncPipeline<TParameter>
        where TParameter : class {
        public AsyncPipeline(IMiddlewareResolver middlewareResolver) : base(middlewareResolver) { }

        public IAsyncPipeline<TParameter> Add<TMiddleware>()
            where TMiddleware : IAsyncMiddleware<TParameter> {
            MiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IAsyncPipeline<TParameter> Add(Type middlewareType) {
            base.AddMiddleware(middlewareType);
            return this;
        }

        #nullable disable
        public async Task Execute(TParameter parameter) {
            if (MiddlewareTypes.Count == 0)
                return;

            int index = 0;
            Func<TParameter, Task> action = null;
            action = async (param) => {
                var type = MiddlewareTypes[index];
                var firstMiddleware = (IAsyncMiddleware<TParameter>)MiddlewareResolver.Resolve(type);

                index++;
                if (index == MiddlewareTypes.Count)
                    action = (p) => Task.FromResult(0);

                await firstMiddleware.Run(param, action).ConfigureAwait(false);
            };

            await action(parameter).ConfigureAwait(false);
        }
        #nullable enable
    }
}
