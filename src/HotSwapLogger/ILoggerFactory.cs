namespace HotSwapLogger
{
    public interface ILoggerFactory
    {
        ILoggingProvider LoggingProvider { get; }
        ILogEventFormatter Formatter { get; }
        ILogger CreateLogger();
        ILoggerFactory AddProvider(ILoggingProvider loggingProvider);
        ILoggerFactory RemoveProvider<TProvider>() where TProvider : ILoggingProvider;
    }
}