using MediatR;

namespace Fridge.Application.Features.ProductImages.Commands.DeleteProductImage;

public sealed record DeleteProductImageCommand(Guid ProductId, Guid ImageId) : IRequest;