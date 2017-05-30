using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using HotSwapLogger;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole();

            var currentPath = Directory.GetCurrentDirectory();
            var watcher = new FileSystemWatcher(currentPath, "*.dll");
            watcher.Created += (sender, args1) => LoadProviderFromDll(loggerFactory, args1.FullPath);
            watcher.Deleted += (sender, args2) => Console.WriteLine($@"Removed {args2.FullPath}");
            watcher.EnableRaisingEvents = true;

            var logger = loggerFactory.CreateLogger();

            logger.Success("Hello World!");

            Console.ReadLine();
        }

        private static void LoadProviderFromDll(LoggerFactory loggerFactory, string path)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            var loaderTypes = assembly.GetExportedTypes()
                .Where(t => typeof(ILoader).IsAssignableFrom(t));
            foreach (var loaderType in loaderTypes)
            {
                var loader = (ILoader) Activator.CreateInstance(loaderType);
                loader.Load(loggerFactory);
            }
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