using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IAppDbContext db)
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            throw new KeyNotFoundException($"Product '{request.Id}' not found.");

        product.Name = request.Name;
        product.DefaultQuantity = request.DefaultQuantity;


        await db.SaveChangesAsync(cancellationToken);
        return new ProductDto(product.Id, product.Name, product.DefaultQuantity);
    }
}