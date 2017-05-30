using System;

namespace HotSwapLogger
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILoggingProvider LoggingProvider { get; private set; }
        public ILogEventFormatter Formatter { get; private set; }

        public ILogger CreateLogger() => new Logger(this);

        public ILoggerFactory AddProvider(ILoggingProvider loggingProvider)
        {
            if (loggingProvider == null)
                throw new ArgumentNullException(nameof(loggingProvider));

            LoggingProvider = LoggingProvider == null 
                ? loggingProvider 
                : new CompositeLoggingProvider(LoggingProvider, loggingProvider);

            return this;
        }

        public ILoggerFactory RemoveProvider<TProvider>() where TProvider : ILoggingProvider
        {
            // TODO: create a dictionary, remove the provider from there and recreate a composite logger

            return this;
        }
    }
}