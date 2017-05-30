using System.IO;

namespace HotSwapLogger.Providers.FileAppender
{
    public class FileAppenderLoggingProvider : ILoggingProvider
    {
        private readonly string _path;

        public FileAppenderLoggingProvider(string path)
        {
            _path = path;
        }

        void ILoggingProvider.Log(LogEvent logEvent, ILogEventFormatter formatter)
        {
            // TODO: move I/O operations to another thread, this should just enqueue the events
            using (var sw = File.Exists(_path) ? File.AppendText(_path) : File.CreateText(_path))
                sw.WriteLine(formatter.Format(logEvent));
        }
    }
}