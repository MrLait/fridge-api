using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await db
            .Products
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new ProductDto(
                x.Id,
                x.Name,
                x.DefaultQuantity
            ))
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new KeyNotFoundException($"Product '{request.Id}' not found.");

        return product;
    }
}