using AlphaKop.Core.Flows.Middleware;
using AlphaKop.Core.Flows.MiddlewareResolver;
using System;

namespace AlphaKop.Core.Flows.Chains {
    public sealed class TaskChain<TParameter, TReturn> : BaseMiddlewareFlow<IMiddleware<TParameter, TReturn>>,
        ITaskChain<TParameter, TReturn> {
        private Func<TParameter, TReturn>? _finallyFunc;

        public TaskChain(IMiddlewareResolver middlewareResolver) : base(middlewareResolver) {
        }

        public ITaskChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFunc) {
            this._finallyFunc = finallyFunc;
            return this;
        }

        public ITaskChain<TParameter, TReturn> Chain<TMiddleware>()
            where TMiddleware : IMiddleware<TParameter, TReturn> {
            MiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public ITaskChain<TParameter, TReturn> Chain(Type middlewareType) {
            base.AddMiddleware(middlewareType);
            return this;
        }

        #nullable disable
        public TReturn Execute(TParameter parameter) {
            if (MiddlewareTypes.Count == 0)
                return default(TReturn);

            int index = 0;
            Func<TParameter, TReturn> func = null;
            func = (param) => {
                var type = MiddlewareTypes[index];
                var middleware = (IMiddleware<TParameter, TReturn>)MiddlewareResolver.Resolve(type);

                index++;
                // If the current instance of middleware is the last one in the list,
                // the "next" function is assigned to the finally function or a 
                // default empty function.
                if (index == MiddlewareTypes.Count)
                    func = this._finallyFunc ?? ((p) => default(TReturn));

                return middleware.Run(param, func);
            };

            return func(parameter);
        }
        #nullable disable
    }
}
