using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace HotSwapLogger.Loader
{
    public class AssemblyLoaderWrapper : IAssemblyLoader
    {
        private readonly string _tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "TEMP");

        AppDomain IAssemblyLoader.Load(ILoggerFactory loggerFactory, NameAndPath nameAndPath)
        {
            var path = nameAndPath.Path;
            if (!Directory.Exists(_tempFolder))
                Directory.CreateDirectory(_tempFolder);

            var copy = Path.Combine(_tempFolder, Path.GetFileName(path));
            File.Copy(path, copy, true);

            var domain = AppDomain.CreateDomain(nameAndPath.Name);

            var proxyType = typeof(Proxy);
            var proxy = (Proxy)domain.CreateInstanceAndUnwrap(
                proxyType.Assembly.FullName,
                proxyType.FullName);

            var assembly = proxy.GetAssembly(copy);
            var exportedTypes = assembly.GetExportedTypes();
            var loaderType = exportedTypes
                .FirstOrDefault(t => typeof(ILoader).IsAssignableFrom(t) && t.IsClass);
            var settingsType = exportedTypes
                .FirstOrDefault(t => typeof(ILoaderSettings).IsAssignableFrom(t) && t.IsClass);

            var loader = (ILoader)Activator.CreateInstance(loaderType);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"{path}.json", optional: true)
                .Build();

            var settings = (ILoaderSettings)configuration.Get(settingsType);

            loader.Load(loggerFactory, settings);

            return domain;
        }

        void IAssemblyLoader.Unload(AppDomain domain, NameAndPath nameAndPath)
        {
            AppDomain.Unload(domain);

            var copy = Path.Combine(_tempFolder, Path.GetFileName(nameAndPath.Path));
            if (File.Exists(copy))
                File.Delete(copy);
        }

        public class Proxy : MarshalByRefObject
        {
            public Assembly GetAssembly(string path) => Assembly.Load(File.ReadAllBytes(path));
        }
    }
}