using Fridge.Infrastructure.Persistence;

namespace Fridge.Api.Extensions;

public static class DatabaseSeedingExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        await dbSeeder.SeedAsync();
    }
}