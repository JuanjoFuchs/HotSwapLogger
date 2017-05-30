using System;

namespace HotSwapLogger.Loader
{
    public interface IAssemblyLoader
    {
        AppDomain Load(ILoggerFactory loggerFactory, NameAndPath nameAndPath);
        void Unload(AppDomain domain, NameAndPath nameAndPath);
    }
}