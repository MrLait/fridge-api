using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
        => await db.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ProductDto(
                x.Id,
                x.Name,
                x.DefaultQuantity
            ))
            .ToListAsync(ct);
}