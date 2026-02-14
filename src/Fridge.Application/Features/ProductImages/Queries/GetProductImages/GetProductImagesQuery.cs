using Fridge.Application.Features.ProductImages.Dtos;
using MediatR;

namespace Fridge.Application.Features.ProductImages.Queries.GetProductImages;

public sealed record GetProductImagesQuery(Guid ProductId, string BaseUrl)
    : IRequest<IReadOnlyList<ProductImageDto>>;