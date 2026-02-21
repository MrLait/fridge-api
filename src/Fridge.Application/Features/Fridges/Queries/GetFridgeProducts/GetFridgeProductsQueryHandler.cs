
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Fridges.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Fridges.Queries.GetFridgeProducts;

public sealed class GetFridgeProductsQueryHandler(IAppDbContext db)
    : IRequestHandler<GetFridgeProductsQuery, IReadOnlyList<FridgeProductDto>>
{
    public async Task<IReadOnlyList<FridgeProductDto>> Handle(GetFridgeProductsQuery request, CancellationToken cancellationToken)
        => await db.FridgeProducts
            .AsNoTracking()
            .Where(x => x.FridgeId == request.FridgeId)
            .Include(x => x.Product)
            .Select(x => new FridgeProductDto(
                x.Id,
                x.ProductId,
                x.Product.Name,
                x.Quantity,
                x.Product.DefaultQuantity))
            .ToListAsync(cancellationToken);
}