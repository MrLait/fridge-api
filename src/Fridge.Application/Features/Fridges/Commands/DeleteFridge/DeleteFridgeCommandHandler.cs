using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Commands.DeleteFridge;

public sealed class DeleteFridgeCommandHandler(IAppDbContext db)
    : IRequestHandler<DeleteFridgeCommand>
{
    public async Task Handle(DeleteFridgeCommand request, CancellationToken ct)
    {
        var fridge = await db.Fridges
            .SingleOrDefaultAsync(x => x.Id == request.Id, ct);

        if (fridge is null)
            throw new KeyNotFoundException($"Fridge '{request.Id}' not found.");

        db.Fridges.Remove(fridge);
        await db.SaveChangesAsync(ct);
        //ToDo на связи FridgeProduct -> Fridge стоит cascade delete
    }
}