using System;

namespace HotSwapLogger
{
    public class ConsoleLoggingProvider : ILoggingProvider
    {
        void ILoggingProvider.Log(LogEvent logEvent, ILogEventFormatter formatter)
            => Console.WriteLine(formatter.Format(logEvent));
    }
}