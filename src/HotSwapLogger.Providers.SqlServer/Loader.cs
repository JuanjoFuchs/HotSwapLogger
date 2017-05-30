namespace HotSwapLogger.Providers.SqlServer
{
    public class Loader : ILoader
    {
        void ILoader.Load(ILoggerFactory loggerFactory, ILoaderSettings loaderSettings)
        {
            var settings = loaderSettings as Settings;
            if (settings == null)
                return;

            loggerFactory.AddSqlServer(settings.ConnectionString);
        }
    }
}