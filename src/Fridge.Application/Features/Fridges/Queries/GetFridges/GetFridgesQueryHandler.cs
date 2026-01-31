
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Fridges.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Queries.GetFridges;

public sealed class GetFridgesQueryHandler(IAppDbContext db)
    : IRequestHandler<GetFridgesQuery, IReadOnlyList<FridgeDto>>
{
    public async Task<IReadOnlyList<FridgeDto>> Handle(GetFridgesQuery request, CancellationToken cancellationToken)
        => await db.Fridges
            .AsNoTracking()
            .Include(x => x.Model)
            .Select(x => new FridgeDto(
                x.Id,
                x.Name,
                x.OwnerName,
                x.ModelId,
                x.Model.Name,
                x.Model.Year))
            .ToListAsync(cancellationToken);
}