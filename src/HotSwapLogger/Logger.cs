using System;

namespace HotSwapLogger
{
    internal class Logger : ILogger
    {
        private readonly ILoggingProvider _provider;
        private readonly ILogEventFormatter _formatter;

        internal Logger(ILoggerFactory loggerFactory)
        {
            _provider = loggerFactory?.LoggingProvider ?? throw new ArgumentNullException(nameof(loggerFactory.LoggingProvider));
            _formatter = loggerFactory?.Formatter ?? new DefaultLogEventFormatter();
        }

        void ILogger.Success(string message) => Log(LogLevel.Success, message);

        void ILogger.Warn(string message) => Log(LogLevel.Warning, message);

        void ILogger.Error(string message) => Log(LogLevel.Error, message);

        private void Log(LogLevel logLevel, string message)
            => _provider.Log(new LogEvent {Level = logLevel, Message = message}, _formatter);
    }
}