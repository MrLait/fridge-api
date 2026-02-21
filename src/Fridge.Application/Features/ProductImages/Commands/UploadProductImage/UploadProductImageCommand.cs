using MediatR;

namespace Fridge.Application.Features.ProductImages.Commands.UploadProductImage;

public sealed record UploadProductImageCommand(
    Guid ProductId,
    string FileName,
    string ContentType,
    long Size,
    Stream Content
) : IRequest<Guid>;