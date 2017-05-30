namespace HotSwapLogger.Providers.FileAppender
{
    public static class FileAppenderLoggerFactoryExtensions
    {
        public static LoggerFactory AddFileAppender(this LoggerFactory loggerFactory, string path)
            => loggerFactory.AddProvider(new FileAppenderLoggingProvider(path));
    }
}