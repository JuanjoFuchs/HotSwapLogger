using HotSwapLogger.Loader;
using Moq;
using Xunit;

namespace HotSwapLogger.Tests.Unit
{
    public class LoggerProviderWatcherTests
    {
        [Fact]
        public void ShouldOnlyLoadAFileOnce()
        {
            var fileWatcher = new Mock<IFileWatcher>();
            var loader = new Mock<IAssemblyLoader>();
            var loggerFactory = new Mock<ILoggerFactory>();
            const string path = "./";

            var loggerProviderWatcher = new LoggerProviderWatcher(fileWatcher.Object, loader.Object);
            loggerProviderWatcher.Start(loggerFactory.Object, path);
            
            fileWatcher.Raise(f => f.Added += null, new NameAndPath{ Name = "Test", Path = "Valid"});
            loader.Verify(
                l => l.Load(loggerFactory.Object, It.Is<NameAndPath>(x => x.Name == "Test" && x.Path == "Valid")),
                Times.Once);
            fileWatcher.Raise(f => f.Added += null, new NameAndPath { Name = "Test", Path = "Other" });
            loader.Verify(
                l => l.Load(loggerFactory.Object, It.Is<NameAndPath>(x => x.Name == "Test" && x.Path == "Other")),
                Times.Never);
        }
    }
}