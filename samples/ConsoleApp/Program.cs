using System;
using System.Collections.Generic;
using System.IO;
using HotSwapLogger;
using HotSwapLogger.Loader;
using HotSwapLogger.Providers.FileAppender;
using HotSwapLogger.Providers.SqlServer;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    class Program
    {
        private const string AppsettingsDirectory = "AppSettings:Path";

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args, new Dictionary<string, string>
                {
                    ["--pathToWatch"]=AppsettingsDirectory,
                    ["-p"]=AppsettingsDirectory,
                })
                .Build();

            var settings = configuration.Get<AppSettings>();
            var pathToWatch = settings?.Path ?? Directory.GetCurrentDirectory();

            var watcher = new FileSystemWatcherWrapper();
            var loader = new AssemblyLoaderWrapper();
            var providerWatcher = new LoggerProviderWatcher(watcher, loader);

            var loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddFileAppender("./log.txt")
                .AddSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=HotSwapLogger; Integrated Security=True;");
            
            providerWatcher.Start(loggerFactory, pathToWatch);

            var logger = loggerFactory.CreateLogger();

            logger.Success("Hello World!");

            Console.ReadLine();
        }
    }
}
