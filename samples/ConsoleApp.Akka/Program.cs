using System;
using System.Linq;
using System.Threading.Tasks;
using HotSwapLogger;
using HotSwapLogger.Providers.FileAppender;
using HotSwapLogger.Providers.SqlServer;

namespace ConsoleApp.Akka
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new LoggerFactory()
                .AddAkkaTransport()
                .AddConsole();

            var logger = factory.CreateLogger();

            logger.Success("Hello World!");

            var exit = false;
            while (!exit)
            {
                logger.Success(@"To add providers:
    add FileAppender ./file_to_log_to.log
    add SqlServer connectionString

To generate sample logs just hit enter.

Action?");
                var line = Console.ReadLine();
                var command = Parse(line);
                if (command != null)
                {
                    command(factory);
                    logger.Success("Done");
                }
                else
                {
                    GenerateLogs(logger, 10).Wait();
                    logger.Success("Do you want to exit? [Y/n]");
                    if ((Console.ReadLine().NullIfEmpty() ?? "y").ToLowerInvariant() == "y")
                        exit = true;
                }
            }
        }

        private static async Task GenerateLogs(ILogger logger, int total)
        {
            var random = new Random();
            for (var i = 1; i <= total; i++)
            {
                var next = random.Next(1, total);
                if (next < i)
                    logger.Warn($"Event {i} out of {total} is lower than {next}");
                else if (next > i)
                    logger.Error($"Event {i} out of {total} is greater than {next}");
                else
                    logger.Success($"Event {i} out of {total} is equal than {next}");

                await Task.Delay(100);
            }
        }

        private static Action<ILoggerFactory> Parse(string line)
        {
            var words = (line ?? string.Empty).Split(' ')
                .Select(w => w.ToLowerInvariant())
                .ToArray();
            if (words.Length < 2)
                return null;

            var action = words[0];

            if (action == "add")
            {
                var provider = GetProvider(words[1], words.Skip(2).ToArray());
                if (provider == null)
                    return null;

                return factory => factory.AddProvider(provider);
            }

            return null;
        }

        private static ILoggingProvider GetProvider(string provider, string[] args)
        {
            switch (provider)
            {
                case "fileappender":
                    if (!args.Any())
                        return null;

                    return new FileAppenderLoggingProvider(args.First());
                case "sqlserver":
                    if (!args.Any())
                        return null;

                    return new SqlServerLoggingProvider(args.First());
                default:
                    return null;
            }
        }
    }
}
