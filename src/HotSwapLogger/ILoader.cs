namespace HotSwapLogger
{
    public interface ILoader
    {
        void Load(ILoggerFactory loggerFactory, ILoaderSettings loaderSettings);
    }
}