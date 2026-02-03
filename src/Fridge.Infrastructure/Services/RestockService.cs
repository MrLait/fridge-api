using Fridge.Application.Common.Exceptions;
using Fridge.Application.Common.Interfaces;
using Fridge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Services;

public sealed class RestockService(AppDbContext db, IFridgeProductService fridgeProductService)
    : IRestockService
{
    public async Task<int> RestockZeroQuantityAsync(CancellationToken ct = default)
    {
        var candidates = await db.RestockCandidateRows
            .FromSqlRaw("EXEC dbo.sp_RestockZeroQuantity")
            .AsNoTracking()
            .ToListAsync(ct);

        if (candidates.Count == 0)
            return 0;

        await using var tx = await db.Database.BeginTransactionAsync(ct);
        var processed = 0;
        foreach (var c in candidates)
        {
            if (c.DefaultQuantity <= 0)
                throw new BusinessRuleViolationException(
                     $"Product '{c.ProductId}' has invalid default quantity '{c.DefaultQuantity}'. Fix products.default_quantity and retry restock.");

            await fridgeProductService.AddProductAsync(c.FridgeId, c.ProductId, c.DefaultQuantity, false, ct);
            processed++;
        }

        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return processed;
    }
}