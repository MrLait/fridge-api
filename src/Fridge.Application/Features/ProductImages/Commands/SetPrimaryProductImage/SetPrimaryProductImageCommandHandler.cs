using Fridge.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Features.ProductImages.Commands.SetPrimaryProductImage;

public sealed class SetPrimaryProductImageCommandHandler(IProductImageService productImageService)
    : IRequestHandler<SetPrimaryProductImageCommand>
{
    public Task Handle(SetPrimaryProductImageCommand request, CancellationToken ct)
        => productImageService.SetPrimaryAsync(request.ProductId, request.ImageId, ct);
}