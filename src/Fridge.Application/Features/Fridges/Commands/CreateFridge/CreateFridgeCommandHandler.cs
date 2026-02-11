using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Commands.CreateFridge;

public sealed class CreateFridgeCommandHandler(IAppDbContext db)
    : IRequestHandler<CreateFridgeCommand, Guid>
{
    public async Task<Guid> Handle(CreateFridgeCommand request, CancellationToken ct)
    {
        var fridgeModelExists = await db
            .FridgeModels
            .AnyAsync(x => x.Id == request.ModelId, ct);

        if (!fridgeModelExists)
            throw new KeyNotFoundException($"FridgeModel '{request.ModelId}' not found.");

        var items = request.InitialProducts ?? [];

        var productIds = items.Select(x => x.ProductId).Distinct().ToList();

        if (productIds.Count > 0)
        {
            var existingProductIds = await db.Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(ct);

            var missingIds = productIds.Except(existingProductIds).ToList();

            if (missingIds.Count > 0)
                throw new KeyNotFoundException($"Some products not found: {string.Join(", ", missingIds)}");
        }
        var fridgeId = Guid.NewGuid();
        var fridge = new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = request.Name,
            OwnerName = request.OwnerName,
            ModelId = request.ModelId
        };

        foreach (var item in items)
        {
            fridge.FridgeProducts.Add(new FridgeProduct()
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        db.Fridges.Add(fridge);
        await db.SaveChangesAsync(ct);

        return fridge.Id;
    }
}