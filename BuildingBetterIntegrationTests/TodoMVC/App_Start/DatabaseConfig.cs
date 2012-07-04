using System;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using NLog;
using TodoMVC.Persistence;

namespace TodoMVC
{
    public class DatabaseConfig
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void CheckDatabaseConnectivity()
        {
            log.Debug("Checking database connectivity");
            using (var dbConnection = DbProviderFactories.GetFactory(SessionProvider.ConnectionSettings.ProviderName).CreateConnection())
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbConnection.ConnectionString = SessionProvider.ConnectionSettings.ConnectionString;
                dbConnection.Open();
                dbCommand.CommandText = "SELECT 1";
                dbCommand.ExecuteScalar();
            }
        }

        public static void MigrateSchema()
        {
            log.Debug("Applying any required schema updates");
            var announcer = new MigrationAnnoucer(message => log.Debug(message), message => log.Error(message));
            var runner = new RunnerContext(announcer)
            {
                Database = "SqlServer",
                Connection = SessionProvider.ConnectionSettings.ConnectionString,
                Target =
                    Path.Combine(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath,
                                 Assembly.GetExecutingAssembly().GetName().Name + ".dll")
            };
            
            new TaskExecutor(runner).Execute();
        }

        private class MigrationAnnoucer : BaseAnnouncer
        {
            private readonly Action<string> writeError;

            public MigrationAnnoucer(Action<string> write, Action<string> error)
                : base(write)
            {
                writeError = error;
            }

            public override void Error(string message)
            {
                writeError(message);
            }
        }
    }
}