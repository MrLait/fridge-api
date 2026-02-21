using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.ProductImages.Commands.DeleteProductImage;

public sealed class DeleteProductImageCommandHandler(IAppDbContext db, IFileStorage storage)
    : IRequestHandler<DeleteProductImageCommand>
{
    public async Task Handle(DeleteProductImageCommand request, CancellationToken ct)
    {
        var img = await db.ProductImages.SingleOrDefaultAsync(
            x => x.Id == request.ImageId && x.ProductId == request.ProductId, ct);

        if (img is null)
            throw new KeyNotFoundException($"Image '{request.ImageId}' not found for product '{request.ProductId}'.");

        await storage.DeleteAsync(img.StorageKey, ct);

        db.ProductImages.Remove(img);
        await db.SaveChangesAsync(ct);
    }
}