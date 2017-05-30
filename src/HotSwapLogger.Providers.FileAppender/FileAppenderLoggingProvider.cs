﻿using System.IO;

namespace HotSwapLogger.Providers.FileAppender
{
    public class FileAppenderLoggingProvider : ILoggingProvider
    {
        private readonly string _path;

        public FileAppenderLoggingProvider(string path)
        {
            _path = path;
        }

        void ILoggingProvider.Log(LogEvent logEvent)
        {
            using (var sw = File.Exists(_path) ? File.AppendText(_path) : File.CreateText(_path))
                sw.WriteLine($"[{logEvent.Level}] {logEvent.Message}");
        }
    }
}