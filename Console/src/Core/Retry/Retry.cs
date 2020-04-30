using System;
using System.Collections.Generic;
using System.Threading;

namespace AlphaKop.Core.Retry {
    public static class Retry {
        public static void Do(
            TimeSpan retryInterval,
            int maxAttemptCount,
            Action action
        ) {
            Do<object?>(retryInterval, maxAttemptCount, () => {
                action();
                return null;
            });
        }

        public static T Do<T>(
            TimeSpan retryInterval,
            int maxAttemptCount,
            Func<T> action
        ) {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++) {
                try {
                    if (attempted > 0) {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                } catch (Exception ex) {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
