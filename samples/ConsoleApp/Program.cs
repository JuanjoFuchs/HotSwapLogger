using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole();

            var logger = loggerFactory.CreateLogger();

            logger.Success("Hello World!");

            Console.ReadLine();
        }
    }

    public interface ILogger
    {
        void Success(string message);
    }

    public class LoggerFactory
    {
        public ILoggingProvider LoggingProvider { get; private set; } = new NullProvider();

        public ILogger CreateLogger() => new MyLogger(this);

        public void AddProvider(ILoggingProvider loggingProvider)
            => LoggingProvider = new CompositeLoggingProvider(LoggingProvider, loggingProvider);
    }

    public class NullProvider : ILoggingProvider
    {
        void ILoggingProvider.Log(LogEvent logEvent)
        {
        }
    }

    public class CompositeLoggingProvider : ILoggingProvider
    {
        private readonly List<ILoggingProvider> _loggingProviders;

        public CompositeLoggingProvider(params ILoggingProvider[] loggingProviders)
        {
            _loggingProviders = loggingProviders.ToList();
        }

        void ILoggingProvider.Log(LogEvent logEvent) => _loggingProviders.ForEach(provider => provider.Log(logEvent));
    }

    public class MyLogger : ILogger
    {
        private readonly ILoggingProvider _provider;

        public MyLogger(LoggerFactory loggerFactory)
        {
            _provider = loggerFactory?.LoggingProvider ?? throw new ArgumentNullException(nameof(loggerFactory.LoggingProvider));
        }

        void ILogger.Success(string message)
        {
            _provider.Log(new LogEvent { Level = LogLevel.Success, Message = message });
        }
    }

    public interface ILoggingProvider
    {
        void Log(LogEvent logEvent);
    }

    public class LogEvent
    {
        public LogLevel Level { get; set; }

        public string Message { get; set; }
    }

    public enum LogLevel
    {
        Success
    }

    public static class ConsoleLoggerFactoryExtensions
    {
        public static LoggerFactory AddConsole(this LoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new ConsoleLoggerProvider());

            return loggerFactory;
        }
    }

    public class ConsoleLoggerProvider : ILoggingProvider
    {
        void ILoggingProvider.Log(LogEvent logEvent) => Console.WriteLine($@"[{logEvent.Level}] {logEvent.Message}");
    }
}