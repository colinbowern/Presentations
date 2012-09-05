using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bits.IntegrationTests.Persistence.Migrations
{
    public class MigrationFacts : IUseFixture<DatabaseFixture>
    {
        private DatabaseFixture database;

        public void SetFixture(DatabaseFixture data)
        {
            database = data;
        }

        [Fact]
        public void CanRollback()
        {
            using (var connection = new SqlConnection(database.ConnectionString))
            {
                int startingCount, migratedCount, rolledBackCount;

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("SELECT COUNT(*) from information_schema.tables", connection, transaction))
                {
                    startingCount = (int)command.ExecuteScalar();
                    transaction.Commit();
                }

                database.UseLatestDatabaseSchema();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("SELECT COUNT(*) from information_schema.tables", connection, transaction))
                {
                    migratedCount = (int)command.ExecuteScalar();
                    transaction.Commit();
                }

                database.RollbackDatabaseSchema();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("SELECT COUNT(*) from information_schema.tables", connection, transaction))
                {
                    rolledBackCount = (int)command.ExecuteScalar();
                    transaction.Commit();
                }

                startingCount.Should().Be(rolledBackCount - 1 /* Version Info Table */).And.BeLessThan(migratedCount);
            }
        }
    }
}
