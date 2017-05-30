namespace HotSwapLogger.Providers.SqlServer
{
    public static class SqlServerLoggerFactoryExtensions
    {
        public static ILoggerFactory AddSqlServer(this ILoggerFactory loggerFactory, string connectionString)
            => loggerFactory.AddProvider(new SqlServerLoggingProvider(connectionString));
    }
}