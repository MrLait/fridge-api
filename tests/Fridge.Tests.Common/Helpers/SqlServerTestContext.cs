using Fridge.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Tests.Common.Helpers;

public sealed class SqlServerTestContext : IAsyncDisposable
{
    public AppDbContext Db { get; }

    private readonly string _serverConnStr;
    private readonly string _dbName;

    private SqlServerTestContext(AppDbContext db, string serverConnStr, string dbName)
    {
        Db = db;
        _serverConnStr = serverConnStr;
        _dbName = dbName;
    }

    public static async Task<SqlServerTestContext> CreateAsync(string serverConnectionString)
    {
        var dbName = $"FridgeTests_{Guid.NewGuid():N}";

        // Нормализуем: используем master для CREATE/DROP и убираем возможные дубли
        var serverCs = new SqlConnectionStringBuilder(serverConnectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        // CREATE DATABASE
        await using (var conn = new SqlConnection(serverCs))
        {
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = $"CREATE DATABASE [{dbName}]";
            await cmd.ExecuteNonQueryAsync();
        }

        // DbContext connection string: та же, но с нужной БД
        var dbCs = new SqlConnectionStringBuilder(serverCs)
        {
            InitialCatalog = dbName
        }.ConnectionString;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(dbCs)
            .Options;

        var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();

        return new SqlServerTestContext(db, serverCs, dbName);
    }

    public async ValueTask DisposeAsync()
    {
        await Db.DisposeAsync();

        // drop database
        await using var conn = new SqlConnection(_serverConnStr);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $@"
ALTER DATABASE [{_dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [{_dbName}];";
        await cmd.ExecuteNonQueryAsync();
    }
}