using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;

public sealed class DeleteFridgeProductCommandHandler(IAppDbContext db)
    : IRequestHandler<DeleteFridgeProductCommand>
{
    public async Task Handle(DeleteFridgeProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await db.FridgeProducts
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new KeyNotFoundException($"FridgeProduct '{request.Id}' not found.");

        db.FridgeProducts.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
    }
}