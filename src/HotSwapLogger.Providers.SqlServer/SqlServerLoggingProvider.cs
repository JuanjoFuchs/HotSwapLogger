using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace HotSwapLogger.Providers.SqlServer
{
    public class SqlServerLoggingProvider : ILoggingProvider
    {
        private const string Table = "LogEvents";
        private readonly string _connectionString;

        public SqlServerLoggingProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        void ILoggingProvider.Log(LogEvent logEvent, ILogEventFormatter formatter)
        {
            // TODO: move I/O operations to another thread, this should just enqueue the events
            using (var db = new SqlConnection(_connectionString))
            {
                var objectId = db.Query<string>("SELECT OBJECT_ID(@Table)", new { Table })
                    .FirstOrDefault();
                if (string.IsNullOrEmpty(objectId))
                    db.Execute(Create(Table));

                db.Execute(
                    $@"INSERT INTO {Table}(Time, {nameof(LogEvent.Level)}, {nameof(LogEvent.Message)}) VALUES(@Time, @Level, @Message)",
                    new { Time = DateTimeOffset.UtcNow, Level = logEvent.Level.ToString(), logEvent.Message });
            }
        }

        private static string Create(string table)
        {
            return $@"
CREATE TABLE dbo.{table}
	(
	Id bigint NOT NULL IDENTITY (1, 1),
	Time datetimeoffset(7) NOT NULL,
	[Level] nvarchar(MAX) NOT NULL,
	Message nvarchar(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
ALTER TABLE dbo.{table} ADD CONSTRAINT
	PK_{table} PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
ALTER TABLE dbo.{table} SET (LOCK_ESCALATION = TABLE)
";
        }
    }
}