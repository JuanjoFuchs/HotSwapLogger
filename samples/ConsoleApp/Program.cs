using System;
using HotSwapLogger;
using HotSwapLogger.Providers.FileAppender;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddFileAppender("./log.txt");

            var logger = loggerFactory.CreateLogger();

            logger.Success("Hello World!");

            Console.ReadLine();
        }
    }

    public static class ConsoleLoggerFactoryExtensions
    {
        public static LoggerFactory AddConsole(this LoggerFactory loggerFactory)
            => loggerFactory.AddProvider(new ConsoleLoggingProvider());
    }

    public class ConsoleLoggingProvider : ILoggingProvider
    {
        void ILoggingProvider.Log(LogEvent logEvent) => Console.WriteLine($@"[{logEvent.Level}] {logEvent.Message}");
    }
}