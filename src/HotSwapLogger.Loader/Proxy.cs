using System;
using System.IO;
using System.Reflection;

namespace HotSwapLogger.Loader
{
    public class Proxy : MarshalByRefObject
    {
        public Assembly GetAssembly(string path) => Assembly.Load(File.ReadAllBytes(path));
    }
}