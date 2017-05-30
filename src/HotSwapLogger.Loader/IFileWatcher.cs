using System;

namespace HotSwapLogger.Loader
{
    public interface IFileWatcher
    {
        event Action<NameAndPath> Added;
        event Action<NameAndPath> Removed;

        void Start(string pathToWatch);
    }
}