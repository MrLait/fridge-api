using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace Fridge.Infrastructure.Tests.Helpers;

public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container =
        new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04")
            .WithPassword("Your_strong_Passw0rd!")
            .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync().WaitAsync(TimeSpan.FromMinutes(5));
        await WaitUntilSqlReadyAsync(ConnectionString, TimeSpan.FromMinutes(2));
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    private static async Task WaitUntilSqlReadyAsync(string cs, TimeSpan timeout)
    {
        var start = DateTime.UtcNow;
        Exception? last = null;

        while (DateTime.UtcNow - start < timeout)
        {
            try
            {
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT 1";
                await cmd.ExecuteScalarAsync();

                return;
            }
            catch (Exception ex)
            {
                last = ex;
                await Task.Delay(1000);
            }
        }

        throw new TimeoutException($"SQL Server did not become ready in {timeout}. Last error: {last}");
    }
}