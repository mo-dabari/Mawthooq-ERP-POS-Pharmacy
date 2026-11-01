using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUp;

namespace Mawthooq.Database.Migrations
{
    public static class DatabaseMigrator
    {
        public static void Migrate(string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To

                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(
                    typeof(DatabaseMigrator).Assembly,
                    name => name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))

                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
                throw new InvalidOperationException("Database migration failed.", result.Error);
        }
    }
}
