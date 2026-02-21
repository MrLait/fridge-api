using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Commands.UpdateFridge;

public sealed class UpdateFridgeCommandHandler(IAppDbContext db)
    : IRequestHandler<UpdateFridgeCommand>
{
    public async Task Handle(UpdateFridgeCommand request, CancellationToken ct)
    {
        var fridge = await db.Fridges
            .SingleOrDefaultAsync(x => x.Id == request.Id, ct);

        if (fridge is null)
            throw new KeyNotFoundException($"Fridge '{request.Id}' not found.");

        if (fridge.ModelId != request.ModelId)
        {
            var fridgeModelExists = await db.FridgeModels
                    .AnyAsync(x => x.Id == request.ModelId, ct);

            if (!fridgeModelExists)
                throw new KeyNotFoundException($"FridgeModel '{request.ModelId}' not found.");
        }

        (fridge.Name, fridge.OwnerName, fridge.ModelId) = (request.Name, request.OwnerName, request.ModelId);

        await db.SaveChangesAsync(ct);
    }
}
