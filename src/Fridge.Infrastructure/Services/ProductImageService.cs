using Fridge.Application.Common.Interfaces;
using Fridge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Services;
public sealed class ProductImageService(AppDbContext db) : IProductImageService
{
    public async Task SetPrimaryAsync(Guid productId, Guid imageId, CancellationToken ct = default)
    {
        var exists = await db.ProductImages.AnyAsync(x => x.Id == imageId && x.ProductId == productId, ct);

        if (!exists)
            throw new KeyNotFoundException($"Image '{imageId}' not found for product '{productId}'.");

        await using var tx = await db.Database.BeginTransactionAsync(ct);

        await db.ProductImages
            .Where(x => x.ProductId == productId && x.IsPrimary)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrimary, false), ct);

        await db.ProductImages
            .Where(x => x.ProductId == productId && x.Id == imageId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrimary, true), ct);

        await tx.CommitAsync(ct);
    }
}