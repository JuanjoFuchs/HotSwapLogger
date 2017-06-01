using Akka.Actor;
using HotSwapLogger;

namespace ConsoleApp.Akka
{
    internal class AkkaProvider : ILoggingProvider
    {
        private readonly IActorRef _actor;

        public AkkaProvider(ILoggingProvider provider, IActorRefFactory actorSystem)
        {
            var props = Props.Create(() => new ActorLogger(provider));
            _actor = actorSystem.ActorOf(props);
        }

        void ILoggingProvider.Log(LogEvent logEvent, ILogEventFormatter formatter)
        {
            _actor.Tell(new LogRequest(logEvent, formatter));
        }
    }
}