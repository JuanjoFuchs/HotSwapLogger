using Akka.Actor;
using HotSwapLogger;

namespace ConsoleApp.Akka
{
    internal class AkkaTransportLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ActorSystem _system;

        public AkkaTransportLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            _system = ActorSystem.Create(nameof(AkkaTransportLoggerFactory));
        }

        ILoggingProvider ILoggerFactory.Provider => _loggerFactory.Provider;
        ILogEventFormatter ILoggerFactory.Formatter => _loggerFactory.Formatter;

        ILogger ILoggerFactory.CreateLogger() => _loggerFactory.CreateLogger();

        ILoggerFactory ILoggerFactory.AddProvider(ILoggingProvider loggingProvider) => _loggerFactory.AddProvider(new AkkaProvider(loggingProvider, _system));

        ILoggerFactory ILoggerFactory.RemoveProvider<TProvider>() => _loggerFactory.RemoveProvider<TProvider>();
    }
}