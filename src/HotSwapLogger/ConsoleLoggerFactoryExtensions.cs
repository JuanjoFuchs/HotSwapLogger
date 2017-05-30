namespace HotSwapLogger
{
    public static class ConsoleLoggerFactoryExtensions
    {
        public static ILoggerFactory AddConsole(this ILoggerFactory loggerFactory)
            => loggerFactory.AddProvider(new ConsoleLoggingProvider());
    }
}