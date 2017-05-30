namespace HotSwapLogger.Providers.FileAppender
{
    public class Loader : ILoader
    {
        void ILoader.Load(ILoggerFactory loggerFactory, ILoaderSettings loaderSettings)
        {
            var settings = loaderSettings as Settings;
            if (settings == null)
                return;

            loggerFactory.AddFileAppender(settings.Path);
        }
    }

    public class Settings : ILoaderSettings
    {
        public string Path { get; set; }
    }
}