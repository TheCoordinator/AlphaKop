using System;

namespace AlphaKop.Core.Flows.MiddlewareResolver {
    public interface IMiddlewareResolver {
        object Resolve(Type type);
    }
}
