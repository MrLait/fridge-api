using System.Security.Cryptography.X509Certificates;
using Fridge.Application.Common.Interfaces;
using Fridge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.ProductImages.Commands.UploadProductImage;

public sealed class UploadProductImageCommandHandler(IAppDbContext db, IFileStorage storage)
    : IRequestHandler<UploadProductImageCommand, Guid>
{
    public async Task<Guid> Handle(UploadProductImageCommand request, CancellationToken ct)
    {
        var productExists = await db.Products
            .AnyAsync(x => x.Id == request.ProductId, ct);

        if (!productExists)
            throw new KeyNotFoundException($"Product '{request.ProductId}' not found.");

        var imageId = Guid.NewGuid();
        var ext = Path.GetExtension(request.FileName);
        var path = $"products/{request.ProductId}/{imageId}{ext}";

        await storage.SaveAsync(path, request.Content, ct);

        var hasPrimary = await db.ProductImages
            .AnyAsync(x => x.IsPrimary && x.ProductId == request.ProductId, ct);

        var entity = new ProductImage
        {
            Id = imageId,
            ProductId = request.ProductId,
            StorageKey = path,
            FileName = request.FileName,
            ContentType = request.ContentType,
            Size = request.Size,
            IsPrimary = !hasPrimary,
            CreatedAt = DateTimeOffset.UtcNow
        };

        db.ProductImages.Add(entity);
        await db.SaveChangesAsync(ct);

        return entity.Id;
    }
}