
using Fridge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;

namespace Fridge.Application.Tests.Helpers;

public class TestContext : IAsyncDisposable
{
    public AppDbContext Db { get; set; }
    private readonly SqliteConnection _connection;

    private TestContext(AppDbContext db, SqliteConnection connection)
    {
        Db = db;
        _connection = connection;
    }

    public static async Task<TestContext> CreateAsync()
    {
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        return new TestContext(db, conn);
    }

    public async ValueTask DisposeAsync()
    {
        await Db.DisposeAsync();
        await _connection.DisposeAsync();
    }
}