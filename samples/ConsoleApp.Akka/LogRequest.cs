using HotSwapLogger;

namespace ConsoleApp.Akka
{
    internal class LogRequest
    {
        public LogEvent LogEvent { get; }
        public ILogEventFormatter Formatter { get; }

        public LogRequest(LogEvent logEvent, ILogEventFormatter formatter)
        {
            LogEvent = logEvent;
            Formatter = formatter;
        }
    }
}