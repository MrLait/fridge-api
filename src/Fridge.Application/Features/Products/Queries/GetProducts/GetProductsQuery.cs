using Fridge.Application.Features.Products.Dtos;
using MediatR;

namespace Fridge.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(string BaseUrl) : IRequest<IReadOnlyList<ProductListDto>>;