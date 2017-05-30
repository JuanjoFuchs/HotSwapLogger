using System;
using System.IO;
using System.Linq;

namespace HotSwapLogger.Loader
{
    public class FileSystemWatcherWrapper : IFileWatcher
    {
        private const string DllFilter = "*.dll";

        public event Action<NameAndPath> Added;
        public event Action<NameAndPath> Removed;

        public void Start(string pathToWatch)
        {
            var fileSystemWatcher = new FileSystemWatcher(pathToWatch, DllFilter);
            fileSystemWatcher.Created += (sender, args) => Added?.Invoke(FileChanged(args.Name, args.FullPath));
            fileSystemWatcher.Deleted += (sender, args) => Removed?.Invoke(FileChanged(args.Name, args.FullPath));

            //InitialScan(pathToWatch);

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static NameAndPath FileChanged(string name, string path) => new NameAndPath {Name = name, Path = path};

        private void InitialScan(string pathToWatch)
        {
            if (Added == null)
                return;

            Directory.EnumerateFiles(pathToWatch, DllFilter, SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(name => Added(FileChanged(name, pathToWatch)));
        }
    }
}