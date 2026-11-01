using Mawthooq.Database.Migrations;

string? connectionString = args.FirstOrDefault() ?? Environment.GetEnvironmentVariable("MAWTHOOQ_CONNECTION");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Pass the connection string as an argument or set MAWTHOOQ_CONNECTION.");
    return 1;
}

DatabaseMigrator.Migrate(connectionString);
Console.WriteLine("Database is up to date.");
return 0;
