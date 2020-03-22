using AlphaKop.Core.Flows.Middleware;
using AlphaKop.Core.Flows.MiddlewareResolver;
using System;
using System.Threading.Tasks;

namespace AlphaKop.Core.Flows.Chains {
    public sealed class AsyncTaskChain<TParameter, TReturn> : BaseMiddlewareFlow<IAsyncMiddleware<TParameter, TReturn>>,
        IAsyncTaskChain<TParameter, TReturn> {
        private Func<TParameter, Task<TReturn>>? finallyFunc;

        public AsyncTaskChain(IMiddlewareResolver middlewareResolver) : base(middlewareResolver) {
        }

        public IAsyncTaskChain<TParameter, TReturn> Chain<TMiddleware>() where TMiddleware : IAsyncMiddleware<TParameter, TReturn> {
            MiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IAsyncTaskChain<TParameter, TReturn> Chain(Type middlewareType) {
            base.AddMiddleware(middlewareType);
            return this;
        }

        #nullable disable
        public async Task<TReturn> Execute(TParameter parameter) {
            if (MiddlewareTypes.Count == 0) {
                return default(TReturn);
            }

            int index = 0;
            Func<TParameter, Task<TReturn>> func = null;
            func = (param) => {
                var type = MiddlewareTypes[index];
                var middleware = (IAsyncMiddleware<TParameter, TReturn>)MiddlewareResolver.Resolve(type);

                index++;
                // If the current instance of middleware is the last one in the list,
                // the "next" function is assigned to the finally function or a 
                // default empty function.
                if (index == MiddlewareTypes.Count)
                    func = this.finallyFunc ?? ((p) => Task.FromResult(default(TReturn)));

                return middleware.Run(param, func!);
            };

            return await func(parameter).ConfigureAwait(false);
        }
        #nullable enable

        public IAsyncTaskChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc) {
            this.finallyFunc = finallyFunc;
            return this;
        }
    }
}
