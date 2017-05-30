namespace HotSwapLogger.Providers.SqlServer
{
    public class Settings : ILoaderSettings
    {
        public string ConnectionString { get; set; }
    }
}