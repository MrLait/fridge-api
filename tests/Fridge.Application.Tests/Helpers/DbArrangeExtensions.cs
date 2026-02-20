using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Helpers;

public static class DbContextExtensions
{
    public static async Task AddAndSaveAsync<T>(this DbContext db, T entity, CancellationToken ct = default)
        where T : class
    {
        db.Add(entity);
        await db.SaveChangesAsync(ct);
    }

    public static async Task AddRangeAndSaveAsync(this DbContext db, CancellationToken ct = default, params object[] entities)
    {
        db.AddRange(entities);
        await db.SaveChangesAsync(ct);
    }
}