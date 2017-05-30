namespace HotSwapLogger
{
    internal class DefaultLogEventFormatter : ILogEventFormatter
    {
        public string Format(LogEvent logEvent) => $@"[{logEvent.Level}] {logEvent.Message}";
    }
}