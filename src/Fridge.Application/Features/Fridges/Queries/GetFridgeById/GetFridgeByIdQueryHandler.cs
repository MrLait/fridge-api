using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Fridges.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Queries.GetFridgeById;

public sealed class GetFridgeByIdQueryHandler(IAppDbContext db)
    : IRequestHandler<GetFridgeByIdQuery, FridgeDto>
{
    public async Task<FridgeDto> Handle(GetFridgeByIdQuery request, CancellationToken ct)
    {
        var dto = await db.Fridges
            .AsNoTracking()
            .Include(x => x.Model)
            .Where(x => x.Id == request.Id)
            .Select(x => new FridgeDto(x.Id, x.Name, x.OwnerName, x.ModelId, x.Model.Name, x.Model.Year))
            .SingleOrDefaultAsync(ct);

        if (dto is null)
            throw new KeyNotFoundException($"Fridge '{request.Id}' not found.");

        return dto;
    }
}