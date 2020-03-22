using System;

namespace AlphaKop.Core.Flows.MiddlewareResolver {
    public sealed class ActivatorMiddlewareResolver : IMiddlewareResolver {
        #nullable disable
        public object Resolve(Type type) {
            return Activator.CreateInstance(type);
        }
        #nullable enable
    }
}
