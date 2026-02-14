using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.ProductImages.Commands.SetPrimaryProductImage;

public sealed class SetPrimaryProductImageCommandHandler(IAppDbContext db)
    : IRequestHandler<SetPrimaryProductImageCommand>
{
    public async Task Handle(SetPrimaryProductImageCommand request, CancellationToken ct)
    {
        var img = await db.ProductImages.SingleOrDefaultAsync(
            x => x.Id == request.ImageId && x.ProductId == request.ProductId, ct);

        if (img is null)
            throw new KeyNotFoundException($"Image '{request.ImageId}' not found for product '{request.ProductId}'.");

        var currentPrimary = await db.ProductImages
            .SingleOrDefaultAsync(x => x.ProductId == request.ProductId && x.IsPrimary, ct);

        if (currentPrimary is not null && currentPrimary.Id != img.Id)
            currentPrimary.IsPrimary = false;

        img.IsPrimary = true;
        await db.SaveChangesAsync(ct);
    }
}