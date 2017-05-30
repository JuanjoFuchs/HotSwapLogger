namespace HotSwapLogger
{
    public interface ILoggingProvider
    {
        void Log(LogEvent logEvent);
    }
}