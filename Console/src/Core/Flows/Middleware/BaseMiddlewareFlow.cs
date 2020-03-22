using AlphaKop.Core.Flows.MiddlewareResolver;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AlphaKop.Core.Flows.Middleware {
    public abstract class BaseMiddlewareFlow<TMiddleware> {
        protected IList<Type> MiddlewareTypes { get; private set; }
        protected IMiddlewareResolver MiddlewareResolver { get; private set; }

        internal BaseMiddlewareFlow(IMiddlewareResolver middlewareResolver) {
            MiddlewareResolver = middlewareResolver ?? throw new ArgumentNullException("middlewareResolver",
                "An instance of IMiddlewareResolver must be provided. You can use ActivatorMiddlewareResolver.");
            MiddlewareTypes = new List<Type>();
        }

        private static readonly TypeInfo MiddlewareTypeInfo = typeof(TMiddleware).GetTypeInfo();

        protected void AddMiddleware(Type middlewareType) {
            if (middlewareType == null) throw new ArgumentNullException("middlewareType");

            bool isAssignableFromMiddleware = MiddlewareTypeInfo.IsAssignableFrom(middlewareType.GetTypeInfo());
            if (!isAssignableFromMiddleware)
                throw new ArgumentException(
                    $"The middleware type must implement \"{typeof(TMiddleware)}\".");

            this.MiddlewareTypes.Add(middlewareType);
        }
    }
}
