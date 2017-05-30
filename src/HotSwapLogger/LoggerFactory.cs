using System;

namespace HotSwapLogger
{
    public class LoggerFactory
    {
        public ILoggingProvider LoggingProvider { get; private set; }

        public ILogger CreateLogger() => new Logger(this);

        public LoggerFactory AddProvider(ILoggingProvider loggingProvider)
        {
            if (loggingProvider == null)
                throw new ArgumentNullException(nameof(loggingProvider));

            LoggingProvider = LoggingProvider == null 
                ? loggingProvider 
                : new CompositeLoggingProvider(LoggingProvider, loggingProvider);

            return this;
        }
    }
}