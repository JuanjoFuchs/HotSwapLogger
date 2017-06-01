using Akka.Actor;
using HotSwapLogger;

namespace ConsoleApp.Akka
{
    public class ActorLogger : ReceiveActor
    {
        public ActorLogger(ILoggingProvider provider)
        {
            Receive<LogRequest>(request => provider.Log(request.LogEvent, request.Formatter));
        }
    }
}