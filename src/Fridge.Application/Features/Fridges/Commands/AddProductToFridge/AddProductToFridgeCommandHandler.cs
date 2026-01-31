using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

public sealed class AddProductToFridgeCommandHandler(IAppDbContext db)
    : IRequestHandler<AddProductToFridgeCommand, Guid>
{
    public async Task<Guid> Handle(AddProductToFridgeCommand request, CancellationToken cancellationToken)
    {
        var fridgeExists = await db.Fridges.AnyAsync(x => x.Id == request.FridgeId, cancellationToken);
        if (!fridgeExists) throw new KeyNotFoundException($"Fridge {request.FridgeId} not found");

        var productExists = await db.Products.AnyAsync(x => x.Id == request.ProductId, cancellationToken);
        if (!productExists) throw new KeyNotFoundException($"Product {request.ProductId} not found");

        var existing = await db.FridgeProducts
            .SingleOrDefaultAsync(x => x.FridgeId == request.FridgeId && x.ProductId == request.ProductId, cancellationToken);

        if (existing is null)
        {
            var entity = new FridgeProduct
            {
                Id = Guid.NewGuid(),
                FridgeId = request.FridgeId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            db.FridgeProducts.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        existing.Quantity += request.Quantity;
        await db.SaveChangesAsync(cancellationToken);
        return existing.Id;
    }
}