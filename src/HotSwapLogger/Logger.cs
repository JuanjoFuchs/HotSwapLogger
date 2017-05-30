using System;

namespace HotSwapLogger
{
    internal class Logger : ILogger
    {
        private readonly ILoggerFactory _loggerFactory;

        internal Logger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        void ILogger.Success(string message) => Log(LogLevel.Success, message);

        void ILogger.Warn(string message) => Log(LogLevel.Warning, message);

        void ILogger.Error(string message) => Log(LogLevel.Error, message);

        private void Log(LogLevel logLevel, string message)
            => _loggerFactory.Provider.Log(new LogEvent {Level = logLevel, Message = message}, _loggerFactory.Formatter);
    }
}