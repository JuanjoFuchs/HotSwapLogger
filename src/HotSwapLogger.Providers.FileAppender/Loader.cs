namespace HotSwapLogger.Providers.FileAppender
{
    public class Loader : ILoader
    {
        void ILoader.Load(LoggerFactory loggerFactory)
        {
            loggerFactory.AddFileAppender("./log.txt");
        }
    }
}