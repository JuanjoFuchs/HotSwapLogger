namespace HotSwapLogger
{
    internal class NullProvider : ILoggingProvider
    {
        void ILoggingProvider.Log(LogEvent logEvent, ILogEventFormatter formatter)
        {
        }
    }
}