using Fridge.Application.Common.Interfaces;
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

        var fridge = new Domain.Entities.Fridge
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerName = request.OwnerName,
            ModelId = request.ModelId
        };

        db.Fridges.Add(fridge);
        await db.SaveChangesAsync(ct);
        return fridge.Id;
    }
}