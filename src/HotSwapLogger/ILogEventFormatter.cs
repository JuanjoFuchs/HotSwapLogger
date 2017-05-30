namespace HotSwapLogger
{
    public interface ILogEventFormatter
    {
        string Format(LogEvent logEvent);
    }
}