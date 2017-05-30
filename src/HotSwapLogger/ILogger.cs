namespace HotSwapLogger
{
    public interface ILogger
    {
        void Success(string message);

        void Warn(string message);

        void Error(string message);
    }
}