using HotSwapLogger;

namespace ConsoleApp.Akka
{
    internal static class AkkaTransportLoggerFactoryExtensions
    {
        public static ILoggerFactory AddAkkaTransport(this ILoggerFactory loggerFactory) => new AkkaTransportLoggerFactory(loggerFactory);
    }
}