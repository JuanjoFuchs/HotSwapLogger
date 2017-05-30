using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HotSwapLogger;
using HotSwapLogger.Loader;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    class Program
    {
        private const string AppsettingsDirectory = "Path";

        static void Main(string[] args)
        {
            var settings = ReadSettings(args);
            var logger = SetupLogger(settings);
            DoStuff(logger).Wait();
        }

        private static async Task DoStuff(ILogger logger)
        {
            const int total = 200;
            await GenerateLogs(logger, total);

            var yes = new[]{"y", "Y", ""}.ToList();
            Console.WriteLine($"Generate {total} more logs? [Y/n]");
            while (yes.Contains(Console.ReadLine() ?? "y"))
            {
                await GenerateLogs(logger, total);
                Console.WriteLine($"Generate {total} more logs? [Y/n]");
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

        private static ILogger SetupLogger(AppSettings settings)
        {
            var defaultDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Providers");
            var pathToWatch = settings?.Path ?? defaultDirectory;
            Console.WriteLine($"Watching path: {pathToWatch}");

            var watcher = new FileSystemWatcherWrapper();
            var loader = new AssemblyLoaderWrapper();
            var providerWatcher = new LoggerProviderWatcher(watcher, loader);

            var loggerFactory = new LoggerFactory()
                .AddConsole();

            providerWatcher.Start(loggerFactory, pathToWatch);

            return loggerFactory.CreateLogger();
        }

        private static AppSettings ReadSettings(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args, new Dictionary<string, string>
                {
                    ["--pathToWatch"] = AppsettingsDirectory,
                    ["-p"] = AppsettingsDirectory,
                })
                .Build();

            return configuration.Get<AppSettings>();
        }
    }
}
