using AlphaKop.Core.Flows.Middleware;
using AlphaKop.Core.Flows.MiddlewareResolver;
using System;

namespace AlphaKop.Core.Flows.Pipelines {
    public sealed class Pipeline<TParameter> : BaseMiddlewareFlow<IMiddleware<TParameter>>, IPipeline<TParameter> {
        public Pipeline(IMiddlewareResolver middlewareResolver) : base(middlewareResolver) { }

        public IPipeline<TParameter> Add<TMiddleware>()
            where TMiddleware : IMiddleware<TParameter> {
            MiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IPipeline<TParameter> Add(Type middlewareType) {
            base.AddMiddleware(middlewareType);
            return this;
        }

        #nullable disable
        public void Execute(TParameter parameter) {
            if (MiddlewareTypes.Count == 0)
                return;

            int index = 0;
            Action<TParameter> action = null;
            action = (param) => {
                var type = MiddlewareTypes[index];
                var middleware = (IMiddleware<TParameter>)MiddlewareResolver.Resolve(type);

                index++;
                if (index == MiddlewareTypes.Count)
                    action = (p) => { };

                middleware.Run(param, action);
            };

            action(parameter);
        }
        #nullable enable
    }
}