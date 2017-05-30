namespace HotSwapLogger.Providers.FileAppender
{
    public static class FileAppenderLoggerFactoryExtensions
    {
        public static ILoggerFactory AddFileAppender(this ILoggerFactory loggerFactory, string path)
            => loggerFactory.AddProvider(new FileAppenderLoggingProvider(path));
    }
}