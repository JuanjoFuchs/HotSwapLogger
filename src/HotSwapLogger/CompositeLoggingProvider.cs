using System.Collections.Generic;
using System.Linq;

namespace HotSwapLogger
{
    public class CompositeLoggingProvider : ILoggingProvider
    {
        private readonly List<ILoggingProvider> _loggingProviders;

        public CompositeLoggingProvider(params ILoggingProvider[] loggingProviders)
        {
            _loggingProviders = loggingProviders.ToList();
        }

        void ILoggingProvider.Log(LogEvent logEvent) => _loggingProviders.ForEach(provider => provider.Log(logEvent));
    }
}