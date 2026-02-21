using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductListDto>>
{
    public async Task<IReadOnlyList<ProductListDto>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var baseUrl = request.BaseUrl.TrimEnd('/');

        return await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.DefaultQuantity,
                PrimaryImageId = p.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => (Guid?)i.Id)
                    .FirstOrDefault()
            })
            .Select(x => new ProductListDto(
                x.Id,
                x.Name,
                x.DefaultQuantity,
                x.PrimaryImageId == null
                    ? null
                    : $"{baseUrl}/api/products/{x.Id}/images/{x.PrimaryImageId}"
            ))
            .ToListAsync(ct);
    }
}