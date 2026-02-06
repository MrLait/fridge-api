using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.FridgeModels.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.FridgeModels.Queries.GetFridgeModels;

public sealed class GetFridgeModelsQueryHandler(IAppDbContext db)
    : IRequestHandler<GetFridgeModelsQuery, IReadOnlyList<FridgeModelDto>>
{
    public async Task<IReadOnlyList<FridgeModelDto>> Handle(GetFridgeModelsQuery request, CancellationToken ct)
    {
        var entities = await db.FridgeModels
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new FridgeModelDto(x.Id, x.Name, x.Year))
            .ToListAsync(ct);

        return entities;
    }
}