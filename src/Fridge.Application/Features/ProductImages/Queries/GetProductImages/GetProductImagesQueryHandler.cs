using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.ProductImages.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.ProductImages.Queries.GetProductImages;

public sealed class GetProductImagesQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProductImagesQuery, IReadOnlyList<ProductImageDto>>
{
    public async Task<IReadOnlyList<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken ct)
    {
        var exists = await db.Products
            .AnyAsync(x => x.Id == request.ProductId, ct);

        if (!exists)
            throw new KeyNotFoundException($"Product '{request.ProductId}' not found.");

        var baseUrl = request.BaseUrl.TrimEnd('/');

        var images = await db.ProductImages
            .AsNoTracking()
            .Where(x => x.ProductId == request.ProductId)
            .OrderByDescending(x => x.IsPrimary)
            .Select(x => new ProductImageDto
            (
                x.Id,
                x.FileName,
                x.ContentType,
                x.Size,
                x.IsPrimary,
                x.CreatedAt,
                $"{baseUrl}/api/products/{request.ProductId}/images/{x.Id}"
            ))
            .ToListAsync(ct);

        return images;
    }
}
