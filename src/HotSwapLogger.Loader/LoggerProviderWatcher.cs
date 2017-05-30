using System;
using System.Collections.Concurrent;

namespace HotSwapLogger.Loader
{
    public class LoggerProviderWatcher
    {
        private readonly ConcurrentDictionary<string, AppDomain> _loadedDomains = new ConcurrentDictionary<string, AppDomain>();

        private readonly IFileWatcher _fileWatcher;
        private readonly IAssemblyLoader _loader;

        public LoggerProviderWatcher(IFileWatcher fileWatcher, IAssemblyLoader loader)
        {
            _fileWatcher = fileWatcher;
            _loader = loader;
        }

        public void Start(ILoggerFactory loggerFactory, string pathToWatch)
        {
            _fileWatcher.Added += nameAndPath => ProviderAdded(loggerFactory, nameAndPath);
            _fileWatcher.Removed += nameAndPath => ProviderRemoved(loggerFactory, nameAndPath);

            _fileWatcher.Start(pathToWatch);
        }

        private void ProviderAdded(ILoggerFactory loggerFactory, NameAndPath nameAndPath)
        {
            _loadedDomains.GetOrAdd(nameAndPath.Name, s => _loader.Load(loggerFactory, nameAndPath));
        }

        private void ProviderRemoved(ILoggerFactory loggerFactory, NameAndPath nameAndPath)
        {
            if (!_loadedDomains.TryRemove(nameAndPath.Name, out AppDomain domain))
                return;

            // TODO: remove and dispose provider before unloading?
            //loggerFactory.RemoveProvider<>()
            _loader.Unload(domain, nameAndPath);
        }
    }
}