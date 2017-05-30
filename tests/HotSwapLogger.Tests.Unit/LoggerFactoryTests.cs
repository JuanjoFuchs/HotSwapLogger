using Moq;
using Xunit;

namespace HotSwapLogger.Tests.Unit
{
    public class LoggerFactoryTests
    {
        [Fact]
        public void ShouldLogToAllProviders()
        {
            var firstProvider = new Mock<ILoggingProvider>();
            var secondProvider = new Mock<ILoggingProvider>();
            var loggerFactory = new LoggerFactory()
                .AddProvider(firstProvider.Object);
            var logger = loggerFactory.CreateLogger();

            logger.Success("Test");
            VerifyOnce(firstProvider, LogLevel.Success, "Test");

            loggerFactory.AddProvider(secondProvider.Object);
            logger.Warn("Test1");
            VerifyOnce(firstProvider, LogLevel.Warning, "Test1");
            VerifyOnce(secondProvider, LogLevel.Warning, "Test1");

            void VerifyOnce(Mock<ILoggingProvider> mock, LogLevel level, string message)
                => mock.Verify(
                    p => p.Log(It.Is<LogEvent>(l => l.Level == level && l.Message == message), It.IsAny<ILogEventFormatter>()),
                    Times.Once);
        }
    }
}