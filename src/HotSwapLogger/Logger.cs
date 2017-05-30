using System;

namespace HotSwapLogger
{
    internal class Logger : ILogger
    {
        private readonly ILoggingProvider _provider;

        internal Logger(LoggerFactory loggerFactory)
        {
            _provider = loggerFactory?.LoggingProvider ?? throw new ArgumentNullException(nameof(loggerFactory.LoggingProvider));
        }

        void ILogger.Success(string message)
        {
            _provider.Log(new LogEvent { Level = LogLevel.Success, Message = message });
        }
    }
}