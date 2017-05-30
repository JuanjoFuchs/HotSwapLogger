using System;

namespace HotSwapLogger
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILoggingProvider Provider { get; private set; } = new NullProvider();

        public ILogEventFormatter Formatter { get; } = new DefaultLogEventFormatter();

        public ILogger CreateLogger() => new Logger(this);

        public ILoggerFactory AddProvider(ILoggingProvider loggingProvider)
        {
            if (loggingProvider == null)
                throw new ArgumentNullException(nameof(loggingProvider));

            Provider = Provider == null 
                ? loggingProvider 
                : new CompositeLoggingProvider(Provider, loggingProvider);

            return this;
        }

        public ILoggerFactory RemoveProvider<TProvider>() where TProvider : ILoggingProvider
        {
            // TODO: create a dictionary, remove the provider from there and recreate a composite logger

            return this;
        }
    }
}